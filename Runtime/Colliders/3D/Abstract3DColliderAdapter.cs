using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Abstract Adapter Component for 3D Colliders.
    /// </summary>
    /// <typeparam name="C">The Collider type.</typeparam>
    public abstract class Abstract3DColliderAdapter<C> : AbstractColliderAdapter where C : Collider
    {
        [SerializeField, Tooltip("The local Collider component.")]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        protected C collider = null;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        public override bool Enabled
        {
            get => base.Enabled;
            set => base.Enabled = collider.enabled = value;
        }

        public override bool IsTrigger
        {
            get => collider.isTrigger;
            set => collider.isTrigger = value;
        }

        public override Vector3 Size => Bounds.size;

        public override Vector3 Center => Bounds.center;

        public override Bounds Bounds => collider.bounds;

        public override Vector3 Offset => Center - transform.position;

        /// <summary>
        /// The local Collider component.
        /// </summary>
        public C Collider => collider;

        protected readonly Collider[] buffer = new Collider[10];

        public override void SyncTransforms() => Physics.SyncTransforms();

        protected abstract bool InternalCast(Vector3 direction, float maxDistance, int layerMask,
            out RaycastHit collisionHit, bool draw);

        protected abstract int InternalOverlap(int layerMask);

        public override bool Raycast(Vector3 origin, Vector3 direction, out IRaycastHit closestHit,
            float distance, int mask, bool draw = false)
        {
            var wasHit = Physics.Raycast(origin, direction, out RaycastHit hit, distance, mask);
            closestHit = new RaycastHit3DAdapter(hit);
            if (draw) hit.Draw(origin, direction, distance);
            return wasHit;
        }

        public override bool Raycasts(Vector3 point1, Vector3 point2, Vector3 direction,
            out IRaycastHit closestHit, float distance, int mask,
            int raysCount = 2, bool draw = false)
        {
            const float skin = 0.0001F;

            closestHit = default;
            raysCount = Mathf.Min(raysCount, MAX_RAYS_COUNT);

            var fractionDivider = Mathf.Max(1, raysCount - 1);
            var fraction = 1F / fractionDivider;
            var closestDistance = distance;
            var pointsDirection = (point2 - point1).normalized;
            var depthDirection = Vector3.Cross(direction, pointsDirection);
            var halfDepth = Vector3.Dot(depthDirection, HalfSize);
            var halfSkinDepth = halfDepth - skin * Mathf.Sign(halfDepth);

            // backward raycasts
            var leftPoint1 = point1 - depthDirection * halfSkinDepth;
            var leftPoint2 = point2 - depthDirection * halfSkinDepth;
            for (int i = 0; i < raysCount; i++)
            {
                var origin = Vector3.Lerp(leftPoint1, leftPoint2, i * fraction);
                var wasHit = Raycast(origin, direction, out IRaycastHit hit, distance, mask, draw);
                var isClosestHit = wasHit && hit.Distance < closestDistance;

                if (isClosestHit)
                {
                    closestDistance = hit.Distance;
                    closestHit = hit;
                }
            }

            // middle raycasts
            for (int i = 0; i < raysCount; i++)
            {
                var origin = Vector3.Lerp(point1, point2, i * fraction);
                var wasHit = Raycast(origin, direction, out IRaycastHit hit, distance, mask, draw);
                var isClosestHit = wasHit && hit.Distance < closestDistance;

                if (isClosestHit)
                {
                    closestDistance = hit.Distance;
                    closestHit = hit;
                }
            }

            // forward raycasts
            var rightPoint1 = point1 + depthDirection * halfSkinDepth;
            var rightPoint2 = point2 + depthDirection * halfSkinDepth;
            for (int i = 0; i < raysCount; i++)
            {
                var origin = Vector3.Lerp(rightPoint1, rightPoint2, i * fraction);
                var wasHit = Raycast(origin, direction, out IRaycastHit hit, distance, mask, draw);
                var isClosestHit = wasHit && hit.Distance < closestDistance;

                if (isClosestHit)
                {
                    closestDistance = hit.Distance;
                    closestHit = hit;
                }
            }

            var hasClosestHit = closestHit != null && closestHit.HasCollider();
            if (hasClosestHit && draw) closestHit.Point.Draw(CLOSEST_COLLISION);
            return hasClosestHit;
        }

        public override bool Cast(Vector3 direction, out IRaycastHit hit, float maxDistance, int layerMask, bool draw = false)
        {
            hit = default;
            var hasCollisions = InternalCast(direction, maxDistance, layerMask, out RaycastHit collisionHit, draw);
            if (hasCollisions) hit = new RaycastHit3DAdapter(collisionHit);
            return hasCollisions;
        }

        public override Vector3 ClosestPoint(Vector3 position) => collider.ClosestPoint(position);

        public override bool TryToGetCollidingComponent<T>(int layerMask, out T component)
        {
            var results = InternalOverlap(layerMask);
            var hasResults = results > 0;
            if (hasResults) return buffer[0].TryGetComponent(out component);

            component = default;
            return false;
        }

        public override int TryToGetCollidingComponents<T>(int layerMask, T[] components)
        {
            var results = InternalOverlap(layerMask);
            var size = Mathf.Min(results, components.Length);

            for (int i = 0; i < size; i++)
            {
                components[i] = buffer[i].GetComponent<T>();
            }
            return size;
        }

        public override bool IsOverlapingPoint(Vector3 point, int mask, bool draw = false)
        {
            var colliders = Physics.OverlapSphere(point, radius: 0.01f, mask);
            var isCollision = colliders.Length > 0 && !colliders[0].isTrigger;

            if (draw)
            {
                var color = isCollision ?
                    ExtensionConstants.COLLISION_ON :
                    ExtensionConstants.COLLISION_OFF;
                point.Draw(color);
            }
            return isCollision;
        }

        #region Editor
        protected override void FindCollider() => collider = GetComponent<C>();
        #endregion
    }
}