using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Component adapter for 3D Colliders.
    /// </summary>
    [AddComponentMenu("Physics/Collider 3D Adapter")]
    public class Collider3DAdapter : AbstractColliderAdapter
    {
        [SerializeField, Tooltip("The Collider component.")]
        private new Collider collider = null;

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

        public override Vector3 Center => Bounds.center;

        public override Vector3 Offset
        {
            get => Center - transform.position;
            set
            {
                if (collider is BoxCollider box) box.center = value;
                else if (collider is CapsuleCollider capsule) capsule.center = value;
                else if (collider is SphereCollider sphere) sphere.center = value;
            }
        }

        public override Vector3 Size
        {
            get => Bounds.size;
            set
            {
                if (collider is BoxCollider box) box.size = value;
                else if (collider is SphereCollider sphere) sphere.radius = value.magnitude;
                else if (collider is CapsuleCollider capsule) capsule.radius = value.magnitude;
            }
        }

        public override Bounds Bounds => collider.bounds;

        private readonly Collider[] smallColliderBuffer = new Collider[1];
        private readonly Collider[] bigColliderBuffer = new Collider[10];

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
            float angleLimit = 0f, int raysCount = 2, bool draw = false)
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

                if (isClosestHit && IsAllowedAngle(hit.Normal, angleLimit))
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

                if (isClosestHit && IsAllowedAngle(hit.Normal, angleLimit))
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

                if (isClosestHit && IsAllowedAngle(hit.Normal, angleLimit))
                {
                    closestDistance = hit.Distance;
                    closestHit = hit;
                }
            }

            var hasClosestHit = closestHit != null && closestHit.HasCollider();
            if (hasClosestHit && draw) closestHit.Point.Draw(CLOSEST_COLLISION);
            return hasClosestHit;
        }

        public override bool Cast(Vector3 direction, out IRaycastHit hit, float maxDistance, int layerMask)
        {
            hit = default;
            var hasCollisions = Physics.BoxCast(Center, HalfSize, direction,
                out RaycastHit collisionHit, transform.rotation, maxDistance, layerMask);
            if (hasCollisions) hit = new RaycastHit3DAdapter(collisionHit);
            return hasCollisions;
        }

        public override Vector3 ClosestPoint(Vector3 position) => collider.ClosestPoint(position);

        public override bool IsColliding(int layerMask) =>
            Physics.CheckBox(Center, HalfSize, transform.rotation, layerMask);

        public override bool TryToGetCollidingComponent<T>(int layerMask, out T component)
        {
            int collisions = Physics.OverlapBoxNonAlloc(Center, HalfSize, smallColliderBuffer, transform.rotation, layerMask);
            var hasCollisions = collisions > 0;
            if (hasCollisions) return smallColliderBuffer[0].TryGetComponent(out component);
            component = default;
            return false;
        }

        public override int TryToGetCollidingComponents<T>(int layerMask, T[] components)
        {
            int collisions = Physics.OverlapBoxNonAlloc(Center, HalfSize, bigColliderBuffer, transform.rotation, layerMask);
            int size = Mathf.Min(collisions, components.Length);

            for (int i = 0; i < size; i++)
            {
                components[i] = bigColliderBuffer[i].GetComponent<T>();
            }
            return size;
        }

        /// <summary>
        /// Checks if the given position is overlapping any 3D Collider.
        /// </summary>
        /// <param name="position">Position to check.</param>
        /// <param name="mask">Layer mask to filter.</param>
        /// <param name="draw">Should draw the collision?</param>
        /// <returns>If colliding.</returns>
        public static bool IsOverlapingPoint(Vector2 position, int mask, bool draw = false)
        {
            const float size = 0.01F;
            var colliders = Physics.OverlapSphere(position, size, mask);
            var isCollision = colliders != null && colliders.Length > 0 && !colliders[0].isTrigger;

            if (draw)
            {
                var color = isCollision ?
                    ExtensionConstants.COLLISION_ON :
                    ExtensionConstants.COLLISION_OFF;
                position.Draw(color, size);
            }

            return isCollision;
        }

        #region Editor
        protected override void FindCollider()
        {
            collider = GetComponent<Collider>();
#if UNITY_EDITOR
            if (CanDisplayEditorDialog() && collider == null)
            {
                var colliders = new System.Type[3]
                {
                    typeof(BoxCollider),
                    typeof(SphereCollider),
                    typeof(CapsuleCollider)
                };
                var msg = $"No 3D Collider was found on '{gameObject.name}'.\nWhich one do you want?";
                var index = UnityEditor.EditorUtility.DisplayDialogComplex("Attention", msg,
                    colliders[0].Name,
                    colliders[1].Name,
                    colliders[2].Name
                );

                collider = gameObject.AddComponent(colliders[index]) as Collider;
            }
#endif
        }
        #endregion
    }
}