using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Adapter Component for 2D Capsule Collider.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [AddComponentMenu("Physics 2D/Capsule Collider 2D Adapter")]
    public sealed class CapsuleCollider2DAdapter : Abstract2DColliderAdapter<CapsuleCollider2D>
    {
        public override Vector3 Size { set => collider.size = value; }

        protected override bool InternalCast(Vector3 direction, float maxDistance, int layerMask,
            out RaycastHit2D collisionHit, bool draw) =>
            collider.Cast(DEFAULT_OFFSET, direction, maxDistance, layerMask, out collisionHit,
                ForwardAngle, minDepth, maxDepth, DEFAULT_SKIN, draw);
    }
}