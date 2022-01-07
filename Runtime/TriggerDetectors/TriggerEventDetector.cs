using UnityEngine;
using UnityEngine.Events;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Listen for trigger enter, update and exit events.
    /// <para>This component uses <see cref="AbstractColliderAdapter"/> to check collisions in 2D or 3D.</para>
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TriggerEventDetector : MonoBehaviour
    {
        [SerializeField, Tooltip("Target layers to trigger collisions.")]
        private LayerMask targetLayers = 0;
        [SerializeField, Tooltip("Collider adapter used to calculate collisions in 2D or 3D.")]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        private AbstractColliderAdapter collider;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        [Header("Events")]
        [SerializeField, Tooltip("Event fired on entering a trigger.")]
        private UnityEvent onEnter = new UnityEvent();
        [SerializeField, Tooltip("Event fired on staying a trigger.")]
        private UnityEvent onStay = new UnityEvent();
        [SerializeField, Tooltip("Event fired on exit a trigger.")]
        private UnityEvent onExit = new UnityEvent();

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
                if (!wasColliding) onEnter.Invoke();
                else onStay.Invoke();
            }
            else if (wasColliding) onExit.Invoke();
        }

        /// <summary>
        /// Adds the given UnityAction to <see cref="onEnter"/>.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public void AddEnterAction(UnityAction action) => onEnter.AddListener(action);

        /// <summary>
        /// Adds the given UnityAction to <see cref="onStay"/>.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public void AddStayAction(UnityAction action) => onStay.AddListener(action);

        /// <summary>
        /// Adds the given UnityAction to <see cref="onExit"/>.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public void AddExitAction(UnityAction action) => onExit.AddListener(action);

        /// <summary>
        /// Removes the given UnityAction to <see cref="onEnter"/>.
        /// </summary>
        /// <param name="action">The action to remove.</param>
        public void RemoveEnterAction(UnityAction action) => onEnter.RemoveListener(action);

        /// <summary>
        /// Removes the given UnityAction to <see cref="onStay"/>.
        /// </summary>
        /// <param name="action">The action to remove.</param>
        public void RemoveStayAction(UnityAction action) => onStay.RemoveListener(action);

        /// <summary>
        /// Removes the given UnityAction to <see cref="onExit"/>.
        /// </summary>
        /// <param name="action">The action to remove.</param>
        public void RemoveExitAction(UnityAction action) => onExit.RemoveListener(action);
    }
}