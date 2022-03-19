using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Adapter Component for 3D Sphere Collider.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SphereCollider))]
    [AddComponentMenu("Physics/Sphere Collider 3D Adapter")]
    public sealed class SphereCollider3DAdapter : Abstract3DColliderAdapter<SphereCollider>
    {
        public override Vector3 Size { set => collider.radius = value.magnitude; }
        public override Vector3 Offset { set => collider.center = value; }

        /// <summary>
        /// <inheritdoc cref="SphereCollider.radius"/>
        /// </summary>
        public float Radius => collider.radius;

        public override bool IsColliding(int layerMask) => collider.IsOverlapping(layerMask);

        protected override bool InternalCast(Vector3 direction, float maxDistance, int layerMask,
            out RaycastHit collisionHit, bool draw) =>
            collider.Cast(DEFAULT_OFFSET, direction, maxDistance, layerMask,
                    out collisionHit, DEFAULT_SKIN, draw);

        protected override int InternalOverlap(int layerMask) =>
            Physics.OverlapSphereNonAlloc(Center, Radius * GetBiggestSizeAxis(), buffer, layerMask);
    }
}