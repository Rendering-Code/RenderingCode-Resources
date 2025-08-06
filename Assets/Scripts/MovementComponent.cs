using UnityEngine;

namespace RenderingCode
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeReference]
        private IMovementBehaviour movementBehaviour;

        private void Update()
        {
            movementBehaviour.Move(transform);
        }
    }
}