using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Adapter Component for 2D Composite Collider.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CompositeCollider2D))]
    [AddComponentMenu("Physics 2D/Composite Collider 2D Adapter")]
    public sealed class CompositeCollider2DAdapter : Abstract2DColliderAdapter<CompositeCollider2D>
    {
        public override Vector3 Size { set { } }
        readonly RaycastHit2D[] hits = new RaycastHit2D[1];

        protected override bool InternalCast(Vector3 direction, float maxDistance, int layerMask,
            out RaycastHit2D collisionHit, bool draw)
        {
            var results = collider.Cast(direction, CreateFilter(layerMask), hits, maxDistance);
            var hasResults = results > 0;
            collisionHit = hasResults ? hits[0] : default;
            return hasResults;
        }
    }
}