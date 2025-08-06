using System;
using UnityEngine;

namespace RenderingCode
{
    [Serializable]
    public class ConstantMovementBehaviour : IMovementBehaviour
    {
        [SerializeField]
        private float speed;
        
        public void Move(Transform transform)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }
}