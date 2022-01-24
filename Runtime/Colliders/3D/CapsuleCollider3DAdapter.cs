using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Adapter Component for 3D Capsule Collider.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CapsuleCollider))]
    [AddComponentMenu("Physics/Capsule Collider 3D Adapter")]
    public sealed class CapsuleCollider3DAdapter : Abstract3DColliderAdapter<CapsuleCollider>
    {
        public override Vector3 Size { set => collider.radius = value.magnitude; }
        public override Vector3 Offset { set => collider.center = value; }

        /// <summary>
        /// <inheritdoc cref="CapsuleCollider.radius"/>
        /// </summary>
        public float Radius => collider.radius;

        public override bool IsColliding(int layerMask) => collider.IsOverlapping(layerMask);

        protected override bool InternalCast(Vector3 direction, float maxDistance, int layerMask,
            out RaycastHit collisionHit, bool draw) =>
            collider.Cast(DEFAULT_OFFSET, direction, maxDistance, layerMask,
                    out collisionHit, DEFAULT_SKIN, draw);

        protected override int InternalOverlap(int layerMask)
        {
            (Vector3 point0, Vector3 point1) = collider.GetPoints();
            return Physics.OverlapCapsuleNonAlloc(point0, point1, Radius, buffer, layerMask);
        }
    }
}