using System;
using UnityEngine;

namespace DefaultNamespace
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ShowReferenceAttribute : PropertyAttribute
    {
    }
}