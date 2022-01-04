using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// A set of parameters for filtering 2D cast results.
    /// </summary>
    [System.Serializable]
    public struct CastFilter2D
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
        public Vector2 Offset { get; set; }

        /// <summary>
        /// The cast direction.
        /// </summary>
        [field: SerializeField, Tooltip("The cast direction.")]
        public Vector2 Direction { get; set; }

        /// <summary>
        /// The maximum cast distance.
        /// </summary>
        [field: SerializeField, Min(0F), Tooltip("The maximum cast distance.")]
        public float Distance { get; set; }

        /// <summary>
        /// The angle of the cast (in degrees).
        /// </summary>
        [field: SerializeField, Tooltip("The angle of the cast (in degrees).")]
        public float Angle { get; set; }

        /// <summary>
        /// Only include objects with a Z coordinate (depth) greater than or equal to it.
        /// </summary>
        [field: SerializeField, Tooltip("Only include objects with a Z coordinate (depth) greater than or equal to it.")]
        public float MinDepth { get; set; }

        /// <summary>
        /// Only include objects with a Z coordinate (depth) less than it.
        /// </summary>
        [field: SerializeField, Tooltip("Only include objects with a Z coordinate (depth) less than it.")]
        public float MaxDepth { get; set; }

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