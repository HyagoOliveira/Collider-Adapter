using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Adapter Component for 3D Box Collider.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    [AddComponentMenu("Physics/Box Collider 3D Adapter")]
    public sealed class BoxCollider3DAdapter : Abstract3DColliderAdapter<BoxCollider>
    {
        public override Vector3 Size { set => collider.size = value; }
        public override Vector3 Offset { set => collider.center = value; }

        public override bool IsColliding(int layerMask) => collider.IsOverlapping(layerMask);

        protected override bool InternalCast(Vector3 direction, float maxDistance, int layerMask,
            out RaycastHit collisionHit, bool draw) =>
            collider.Cast(DEFAULT_OFFSET, direction, maxDistance, layerMask, out collisionHit,
                DEFAULT_SKIN, draw);

        protected override int InternalOverlap(int layerMask) =>
            Physics.OverlapBoxNonAlloc(Center, HalfSize, buffer, transform.rotation, layerMask);
    }
}