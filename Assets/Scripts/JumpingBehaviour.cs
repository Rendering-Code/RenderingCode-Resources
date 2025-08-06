using System;
using UnityEngine;

namespace RenderingCode
{
    [Serializable]
    public class JumpingBehaviour : IMovementBehaviour
    {
        [SerializeField]
        private float jumpIntensity = 5f;
        
        private bool isJumping = false;
        private float velocity = 0f;
        
        public void Move(Transform transform)
        {
            var wasJumping = isJumping;
            isJumping = transform.position.y < 0 && velocity < 0f;

            if (!wasJumping && isJumping)
                velocity = jumpIntensity;

            velocity += Time.deltaTime * Physics.gravity.y;
            
            transform.position += Vector3.up * velocity * Time.deltaTime;
        }
    }
}