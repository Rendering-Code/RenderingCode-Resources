using System;
using UnityEngine;

namespace RenderingCode
{
    [Serializable]
    public class CombinedMovementBehaviour : IMovementBehaviour
    {
        [SerializeReference]
        private IMovementBehaviour[] movementBehaviours;

        public CombinedMovementBehaviour() : this(Array.Empty<IMovementBehaviour>()) {}
        
        public CombinedMovementBehaviour(IMovementBehaviour[] movementBehaviours)
        {
            this.movementBehaviours = movementBehaviours;
        }

        public void Move(Transform transform)
        {
            foreach (var behaviour in movementBehaviours)
            {
                behaviour.Move(transform);
            }
        }
    }
}