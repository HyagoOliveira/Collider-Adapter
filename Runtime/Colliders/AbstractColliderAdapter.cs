using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Abstract Adapter Component for Colliders.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class AbstractColliderAdapter : MonoBehaviour, ICollider
    {
        public const int MAX_RAYS_COUNT = 64;
        public const float DEFAULT_SKIN = 0F;
        public static readonly Vector3 DEFAULT_OFFSET = Vector3.zero;
        public static readonly Color CLOSEST_COLLISION = Color.yellow;

        public virtual bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public abstract bool IsTrigger { get; set; }

        public abstract Bounds Bounds { get; }

        public abstract Vector3 Center { get; }

        public abstract Vector3 Offset { get; set; }

        public abstract Vector3 Size { get; set; }

        public Vector3 HalfSize => Bounds.extents;

        protected virtual void Reset() => FindCollider();

        public abstract void SyncTransforms();

        public abstract Vector3 ClosestPoint(Vector3 position);

        public abstract bool IsColliding(int layerMask);

        public abstract bool TryToGetCollidingComponent<T>(int layerMask, out T component);

        public abstract int TryToGetCollidingComponents<T>(int layerMask, T[] components);

        public abstract bool Cast(Vector3 direction, out IRaycastHit hit, float maxDistance, int layerMask, bool draw = false);

        /// <summary>
        /// Casts a Ray against Colliders in the Scene, gathering 
        /// information about the first Collider to contact with.
        /// </summary>
        /// <param name="origin">The Raycast origin.</param>
        /// <param name="direction">The Raycast direction.</param>
        /// <param name="closestHit">The cast information about the first detected object.</param>
        /// <param name="distance">The Raycast distance.</param>
        /// <param name="mask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="draw">Draws the raycast if enabled.</param>
        /// <returns>Whether the Raycast hits any collider in the Scene.</returns>
        public abstract bool Raycast(Vector3 origin, Vector3 direction, out IRaycastHit closestHit,
            float distance, int mask, bool draw = false);

        public virtual bool Raycasts(Vector3 point1, Vector3 point2, Vector3 direction,
            out IRaycastHit closestHit, float distance, int mask,
            int raysCount = 2, bool draw = false)
        {
            closestHit = default;
            raysCount = Mathf.Min(raysCount, MAX_RAYS_COUNT);

            var fractionDivider = Mathf.Max(1, raysCount - 1);
            var fraction = 1F / fractionDivider;
            var closestDistance = distance;

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

            var hasClosestHit = closestHit != null && closestHit.HasCollider();
            if (hasClosestHit && draw) closestHit.Point.Draw(CLOSEST_COLLISION);
            return hasClosestHit;
        }

        /// <summary>
        /// Checks if the given bounds is completely inside this collider.
        /// </summary>
        /// <param name="bounds">A Bounds structure to check.</param>
        /// <returns>True if inside this collider. False otherwise.</returns>
        public bool IsInside(Bounds bounds) => Bounds.Contains(bounds.min) && Bounds.Contains(bounds.max);

        /// <summary>
        /// Checks if overlapping with the given bounds.
        /// </summary>
        /// <param name="bounds">A Bounds structure to check.</param>
        /// <returns>True if overlapping with the bounds. False otherwise.</returns>
        public bool IsOverlapping(Bounds bounds) => Bounds.Contains(bounds.min) || Bounds.Contains(bounds.max);

        public float GetBiggestSizeAxis()
        {
            float maxBetweenXOrY = Mathf.Max(Size.x, Size.y);
            return Mathf.Max(maxBetweenXOrY, Size.z);
        }

        #region Editor
        protected abstract void FindCollider();

        /// <summary>
        /// Displays a message for the user to choose which Collider component to use.
        /// <para><b>This function should only be used on Editor time</b>, like MonoBehaviour.Reset()</para>
        /// </summary>
        /// <param name="gameObject">A GameObject to add a ColliderAdapter implementation.</param>
        /// <returns>An implementation of <see cref="AbstractColliderAdapter"/>.</returns>
        public static AbstractColliderAdapter ResolveCollider(GameObject gameObject) =>
            ColliderAdapterFactory.GetAdapter(gameObject);
        #endregion
    }
}