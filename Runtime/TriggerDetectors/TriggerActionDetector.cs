using System;
using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Listen for trigger enter, update and exit actions.
    /// <para>This component uses <see cref="AbstractColliderAdapter"/> to check collisions in 2D or 3D.</para>
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TriggerActionDetector : MonoBehaviour
    {
        [SerializeField, Tooltip("Target layers to trigger collisions.")]
        private LayerMask targetLayers = 0;
        [SerializeField, Tooltip("Collider adapter used to calculate collisions in 2D or 3D.")]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        private AbstractColliderAdapter collider;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        /// <summary>
        /// Action fired on entering a trigger.
        /// </summary>
        public event Action OnEnter;

        /// <summary>
        /// Action fired on staying a trigger.
        /// </summary>
        public event Action OnStay;

        /// <summary>
        /// Action fired on exit a trigger.
        /// </summary>
        public event Action OnExit;

        /// <summary>
        /// Whether is colliding.
        /// </summary>
        public bool IsColliding { get; private set; }

        private void Reset()
        {
            collider = AbstractColliderAdapter.ResolveCollider(gameObject);
            collider.IsTrigger = true;
        }

        private void Update()
        {
            var wasColliding = IsColliding;
            IsColliding = collider.IsColliding(targetLayers);

            if (IsColliding)
            {
                if (!wasColliding) OnEnter?.Invoke();
                else OnStay?.Invoke();
            }
            else if (wasColliding) OnExit?.Invoke();
        }
    }
}