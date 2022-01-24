using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Adapter Component for 2D Circle Collider.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CircleCollider2D))]
    [AddComponentMenu("Physics 2D/Circle Collider 2D Adapter")]
    public sealed class CircleCollider2DAdapter : Abstract2DColliderAdapter<CircleCollider2D>
    {
        public override Vector3 Size { set => collider.radius = value.magnitude; }

        /// <summary>
        /// <inheritdoc cref="CircleCollider2D.radius"/>
        /// </summary>
        public float Radius => collider.radius;

        protected override bool InternalCast(Vector3 direction, float maxDistance, int layerMask,
            out RaycastHit2D collisionHit, bool draw) =>
            collider.Cast(DEFAULT_OFFSET, direction, maxDistance, layerMask, out collisionHit,
                minDepth, maxDepth, DEFAULT_SKIN, draw);
    }
}
