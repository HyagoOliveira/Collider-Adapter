using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Component adapter for 2D Colliders.
    /// </summary>
    [AddComponentMenu("Physics 2D/Collider 2D Adapter")]
    public class Collider2DAdapter : AbstractColliderAdapter
    {
        [SerializeField, Tooltip("The local Collider2D component.")]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        private Collider2D collider = null;
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

        /// <summary>
        /// The forward Euler angle.
        /// </summary>
        public float ForwardAngle
        {
            get => transform.eulerAngles.z;
            set => transform.Rotate(transform.forward, value);
        }

        public override Vector3 Size
        {
            get => Bounds.size;
            set
            {
                if (collider is BoxCollider2D box) box.size = value;
                else if (collider is CapsuleCollider2D capsule) capsule.size = value;
                else if (collider is CircleCollider2D circle) circle.radius = value.magnitude;
            }
        }

        public override Vector3 Offset
        {
            get => collider.offset;
            set => collider.offset = value;
        }

        public override Vector3 Center => Bounds.center;

        public override Bounds Bounds => collider.bounds;

        private readonly Collider2D[] colliderBuffer = new Collider2D[10];

        protected override void Reset()
        {
            base.Reset();
            CheckQueriesStartingInColliders();
        }

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
            var hasCollisions = false;
            RaycastHit2D collisionHit = default;

            if (collider is BoxCollider2D box)
            {
                var angle = transform.eulerAngles.z;
                box.Cast(DEFAULT_OFFSET, direction, maxDistance, layerMask,
                    out collisionHit, angle, minDepth, maxDepth, DEFAULT_SKIN, draw);
            }
            else if (collider is CircleCollider2D circle)
            {
                circle.Cast(DEFAULT_OFFSET, direction, maxDistance, layerMask,
                    out collisionHit, minDepth, maxDepth, DEFAULT_SKIN, draw);
            }

            if (hasCollisions) hit = new RaycastHit2DAdapter(collisionHit);
            return hasCollisions;
        }

        public override Vector3 ClosestPoint(Vector3 position) => collider.ClosestPoint(position);

        public override bool IsColliding(int layerMask) => GetOverlappingCollider(layerMask) != null;

        public override bool TryToGetCollidingComponent<T>(int layerMask, out T component)
        {
            var collider = GetOverlappingCollider(layerMask);
            if (collider) return collider.TryGetComponent(out component);
            component = default;
            return false;
        }

        public override int TryToGetCollidingComponents<T>(int layerMask, T[] components)
        {
            int collisions = Physics2D.OverlapBoxNonAlloc(Center, Size, ForwardAngle,
                colliderBuffer, layerMask, minDepth, maxDepth);
            int size = Mathf.Min(collisions, components.Length);

            for (int i = 0; i < size; i++)
            {
                components[i] = colliderBuffer[i].GetComponent<T>();
            }
            return size;
        }

        /// <summary>
        /// Checks if the given position is overlapping any 2D Collider.
        /// <para>
        /// Your collider should be used by a Composite Collider with a GeometryType 
        /// set to <see cref="CompositeCollider2D.GeometryType.Polygons"/>
        /// </para>
        /// </summary>
        /// <param name="position">Position to check.</param>
        /// <param name="mask">Layer mask to filter.</param>
        /// <param name="draw">Should draw the collision?</param>
        /// <returns>Whether is colliding.</returns>
        public static bool IsOverlapingPoint(Vector2 position, int mask, bool draw = false)
        {
            var collider = Physics2D.OverlapPoint(position, mask);
            var isCollision = collider && !collider.isTrigger;

            if (draw)
            {
                var color = isCollision ?
                    ExtensionConstants.COLLISION_ON :
                    ExtensionConstants.COLLISION_OFF;
                position.Draw(color);
            }
            return isCollision;
        }

        private Collider2D GetOverlappingCollider(int layerMask) =>
             Physics2D.OverlapBox(Center, Size, ForwardAngle, layerMask, minDepth, maxDepth);

        #region Editor
        protected override void FindCollider()
        {
            collider = GetComponent<Collider2D>();
#if UNITY_EDITOR
            if (CanDisplayEditorDialog() && collider == null)
            {
                var colliders = new System.Type[3]
                {
                    typeof(BoxCollider2D),
                    typeof(CircleCollider2D),
                    typeof(CapsuleCollider2D)
                };
                var msg = $"No 2D Collider was found on '{gameObject.name}'.\nWhich one do you want?";
                var index = UnityEditor.EditorUtility.DisplayDialogComplex("Attention", msg,
                    colliders[0].Name,
                    colliders[1].Name,
                    colliders[2].Name
                );

                collider = gameObject.AddComponent(colliders[index]) as Collider2D;
            }
#endif
        }

        private static void CheckQueriesStartingInColliders()
        {
            var isQueriesDisabled = !Physics2D.queriesStartInColliders;
            if (isQueriesDisabled || !CanDisplayEditorDialog()) return;
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