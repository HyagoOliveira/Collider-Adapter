using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// A set of parameters for filtering 3D cast results.
    /// </summary>
    [System.Serializable]
    public struct CastFilter3D
    {
        /// <summary>
        /// Filter to detect Colliders only on certain layers.
        /// </summary>
        [field: SerializeField, Tooltip("Filter to detect Colliders only on certain layers.")]
        public LayerMask Collisions { get; set; }

        /// <summary>
        /// The offset position from the origin.
        /// </summary>
        [field: SerializeField, Tooltip("The offset position from the origin.")]
        public Vector3 Offset { get; set; }

        /// <summary>
        /// The cast direction.
        /// </summary>
        [field: SerializeField, Tooltip("The cast direction.")]
        public Vector3 Direction { get; set; }

        /// <summary>
        /// The maximum cast distance.
        /// </summary>
        [field: SerializeField, Min(0F), Tooltip("The maximum cast distance.")]
        public float Distance { get; set; }

        /// <summary>
        /// A small value that decreases the cast size. Expands it if negative.
        /// </summary>
        [field: SerializeField, Tooltip("A small value that decreases the cast size. Expands it if negative.")]
        public float Skin { get; set; }

        /// <summary>
        /// Draws the cast if enabled.
        /// </summary>
        [field: SerializeField, Tooltip("Draws the cast if enabled.")]
        public bool Draw { get; set; }
    }
}