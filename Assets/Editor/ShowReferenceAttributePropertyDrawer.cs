using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowReferenceAttribute))]
public class ShowReferenceAttributePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var interfaceType = Type.GetType(string.Join(", ", property.managedReferenceFieldTypename.Split(" ").Reverse()));
        var possibleClasses = Assembly.GetAssembly(interfaceType)
            .GetTypes()
            .Where(x => x.GetInterfaces().Contains(interfaceType) && !x.IsInterface)
            .ToDictionary(x => x.Name, x => x);
        
        var possibleClassesNames = possibleClasses.Keys.ToArray();
        var previousIndex = property.managedReferenceValue != null ? Array.IndexOf(possibleClassesNames, property.managedReferenceValue.GetType().Name) : -1;
        
        EditorGUI.DrawRect(position, new Color(0.25f, 0.25f, 0.25f, 1f));
        
        position.height = EditorGUIUtility.singleLineHeight;
        var index = EditorGUI.Popup(position, property.displayName, previousIndex, possibleClassesNames);
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        if (previousIndex != index)
        {
            var className = possibleClassesNames[index];
            var instance = Activator.CreateInstance(possibleClasses[className]);
            property.managedReferenceValue = instance;
        }

        if (property.managedReferenceValue != null)
        {
            var reference = property.managedReferenceValue;
            foreach (var field in GetSerializedFields(reference.GetType()))
                DrawGUI(reference, field, ref position);
            property.managedReferenceValue = reference;
        }
    }

    private void DrawGUI(object reference, FieldInfo field, ref Rect position)
    {
        if (field.FieldType == typeof(int))
            ModifyType<int>(reference, field, ref position, EditorGUI.IntField);
        else if (field.FieldType == typeof(float))
            ModifyType<float>(reference, field, ref position, EditorGUI.FloatField);
        else if (field.FieldType == typeof(string))
            ModifyType<string>(reference, field, ref position, EditorGUI.TextField);
        else if (field.FieldType == typeof(bool))
            ModifyType<bool>(reference, field, ref position, EditorGUI.Toggle);
        else if (field.FieldType == typeof(Vector2))
            ModifyType<Vector2>(reference, field, ref position, EditorGUI.Vector2Field);
        else if (field.FieldType == typeof(Vector3))
            ModifyType<Vector3>(reference, field, ref position, EditorGUI.Vector3Field);
        else if (field.FieldType == typeof(Vector4))
            ModifyType<Vector4>(reference, field, ref position, EditorGUI.Vector4Field);
    }

    private void ModifyType<T>(object reference, FieldInfo field, ref Rect position, Func<Rect, string, T, T> drawFunction)
    {
        T value = (T)field.GetValue(reference);
        field.SetValue(reference, drawFunction(position, field.Name, value));
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = EditorGUIUtility.singleLineHeight;

        if (property.managedReferenceValue != null)
        {
            var fields = GetSerializedFields(property.managedReferenceValue.GetType());
            foreach (var _ in fields)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
        
        return height;
    }

    private List<FieldInfo> GetSerializedFields(Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.HasAttribute<SerializeField>()).ToList();
    }
}
