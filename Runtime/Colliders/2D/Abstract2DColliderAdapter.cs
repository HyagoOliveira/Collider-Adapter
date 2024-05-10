using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Abstract Adapter Component for 2D Colliders.
    /// </summary>
	/// <typeparam name="C">The Collider type.</typeparam>
    public abstract class Abstract2DColliderAdapter<C> : AbstractColliderAdapter where C : Collider2D
    {
        [SerializeField, Tooltip("The local Collider component.")]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        protected C collider = null;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        [Tooltip("Only detect objects with a Z coordinate (depth) greater than this value.")]
        public float minDepth = 0;
        [Tooltip("Only detect objects with a Z coordinate (depth) less than this value.")]
        public float maxDepth = 0;

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

        public override Vector3 Offset
        {
            get => collider.offset;
            set => collider.offset = value;
        }

        public override Vector3 Center => Bounds.center;

        public override Bounds Bounds => collider.bounds;

        /// <summary>
        /// The local Collider component.
        /// </summary>
        public C Collider => collider;

        /// <summary>
        /// The forward Euler angle.
        /// </summary>
        public float ForwardAngle
        {
            get => transform.eulerAngles.z;
            set => transform.Rotate(transform.forward, value);
        }

        protected readonly Collider2D[] buffer = new Collider2D[10];

        protected override void Reset()
        {
            base.Reset();
            CheckQueriesStartingInColliders();
        }

        public override void SyncTransforms() => Physics2D.SyncTransforms();

        protected abstract bool InternalCast(Vector3 direction, float maxDistance, int layerMask,
            out RaycastHit2D collisionHit, bool draw);

        public override bool Raycast(Vector3 origin, Vector3 direction, out IRaycastHit closestHit,
           float distance, int mask, bool draw = false)
        {
            var hit = Physics2D.Raycast(origin, direction, distance, mask, minDepth, maxDepth);
            closestHit = new RaycastHit2DAdapter(hit);
            if (draw) hit.Draw(origin, direction, distance);
            return hit;
        }

        public override bool Cast(Vector3 direction, out IRaycastHit hit, float maxDistance, int layerMask, bool draw = false)
        {
            hit = default;
            var hasCollisions = InternalCast(direction, maxDistance, layerMask, out RaycastHit2D collisionHit, draw);
            if (hasCollisions) hit = new RaycastHit2DAdapter(collisionHit);
            return hasCollisions;
        }

        public override Vector3 ClosestPoint(Vector3 position) => collider.ClosestPoint(position);

        public override bool IsColliding(int layerMask)
        {
            var hits = collider.OverlapCollider(CreateFilter(layerMask), buffer);
            var hasHits = hits > 0;
            return hasHits;
        }

        public override bool TryToGetCollidingComponent<T>(int layerMask, out T component)
        {
            var isColliding = IsColliding(layerMask);
            if (isColliding) return buffer[0].TryGetComponent(out component);

            component = default;
            return false;
        }

        public override int TryToGetCollidingComponents<T>(int layerMask, T[] components)
        {
            var results = collider.OverlapCollider(CreateFilter(layerMask), buffer);
            var size = Mathf.Min(results, components.Length);

            for (int i = 0; i < size; i++)
            {
                components[i] = buffer[i].GetComponent<T>();
            }
            return size;
        }

        public override bool TryGetOverlapingComponent<T>(Vector3 point, int layerMask, out T component, bool draw = false)
        {
            component = default;
            var hasCollider = TryGetOverlapingCollider(point, layerMask, out Collider2D collider, draw);
            return hasCollider && collider.TryGetComponent(out component);
        }

        /// <summary>
        /// Checks if the given point is overlapping any 2D Collider.
        /// <para>
        /// Your collider should be used by a Composite Collider with a GeometryType 
        /// set to <see cref="CompositeCollider2D.GeometryType.Polygons"/>
        /// </para>
        /// </summary>
        /// <param name="point">Position to check.</param>
        /// <param name="mask">Layer mask to filter.</param>
        /// <param name="draw">Should draw the collision?</param>
        /// <returns>Whether is colliding.</returns>
        public override bool IsOverlapingPoint(Vector3 point, int mask, bool draw = false) =>
            TryGetOverlapingCollider(point, mask, out Collider2D _, draw);

        /// <summary>
        /// Tries to get any overlapping 2D Collider using the given point and mask.
        /// <para>
        /// Your collider should be used by a Composite Collider with a GeometryType 
        /// set to <see cref="CompositeCollider2D.GeometryType.Polygons"/>
        /// </para>
        /// </summary>
        /// <param name="point">Position to check.</param>
        /// <param name="mask">Layer mask to filter.</param>
        /// <param name="collider">The overlapping collider if any.</param>
        /// <param name="draw">Should draw the collision?</param>
        /// <returns>Whether the given point is overlapping the Collider.</returns>
        public bool TryGetOverlapingCollider(Vector3 point, int mask, out Collider2D collider, bool draw = false)
        {
            collider = Physics2D.OverlapPoint(point, mask);
            var isCollision = collider && !collider.isTrigger;

            if (draw)
            {
                var color = isCollision ?
                    ExtensionConstants.COLLISION_ON :
                    ExtensionConstants.COLLISION_OFF;
                point.Draw(color);
            }
            return isCollision;
        }

        protected ContactFilter2D CreateFilter(int layerMask)
        {
            var filter = new ContactFilter2D
            {
                useLayerMask = true,
                useDepth = true,

                layerMask = layerMask,
                minDepth = minDepth,
                maxDepth = maxDepth
            };
            return filter;
        }

        #region Editor
        protected override void FindCollider() => collider = GetComponent<C>();

        protected static void CheckQueriesStartingInColliders()
        {
            var isQueriesDisabled = !Physics2D.queriesStartInColliders;
            if (isQueriesDisabled || !ColliderAdapterFactory.CanDisplayEditorDialog()) return;
#if UNITY_EDITOR
            const string title = "Attention";
            const string yesButton = "Yes, please!";
            const string noButton = "No, Thanks.";
            const string message = "Your project has 'Queries Start In Colliders' enabled." +
                "This means that Raycasts/Linecasts that start inside this Collider will detect itself.\n" +
                "To disable that, please go to Editor > Project Settings > Physics 2D and disable Queries Start In Colliders.\n\n" +
                "We can disable it for you right now. Do you want that?";

            var disable = UnityEditor.EditorUtility.DisplayDialog(title, message, yesButton, noButton);
            if (disable)
            {
                Physics2D.queriesStartInColliders = false;
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.EditorUtility.DisplayDialog("Queries Start In Colliders", "Queries Start In Colliders was disabled!", "Okay");
            }
#endif
        }
        #endregion
    }
}