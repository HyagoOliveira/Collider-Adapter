using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Factory class for Collider Adapters.
    /// </summary>
    public static class ColliderAdapterFactory
    {
        /// <summary>
        /// Gets the best implementation of <see cref="AbstractColliderAdapter"/> based on given GameObject.
        /// <para>A message will be displayed for the user to choose which Adapter to use if no Collider is found.</para>
        /// <para><b>This function should only be used on Editor time</b>, like MonoBehaviour.Reset()</para>
        /// </summary>
        /// <param name="gameObject">A GameObject to add a ColliderAdapter implementation.</param>
        /// <returns>An implementation of <see cref="AbstractColliderAdapter"/>.</returns>
        public static AbstractColliderAdapter GetAdapter(GameObject gameObject)
        {
            var adapter = gameObject.GetComponent<AbstractColliderAdapter>();
            if (adapter) return adapter;

            var hasCollider3D = gameObject.TryGetComponent(out Collider collider3D);
            var hasCollider2D = gameObject.TryGetComponent(out Collider2D collider2D);

            if (hasCollider2D) return GetAdapter2D(collider2D);
            if (hasCollider3D) return GetAdapter3D(collider3D);

#if UNITY_EDITOR
            if (CanDisplayEditorDialog())
            {
                const string title = "Adding required Collider";
                const string message = "You need a Collider component.\nChoose one of the following options:";

                var use2D = UnityEditor.EditorUtility.DisplayDialog(title, message, "Use for 2D", "Use for 3D");
                adapter = use2D ? AddAdapter2D(gameObject) : AddAdapter3D(gameObject);
            }
#endif
            return adapter;
        }

        /// <summary>
        /// Gets the best implementation of <see cref="AbstractColliderAdapter"/> 
        /// as a 2D Collider Adapter based on given GameObject.
        /// </summary>
        /// <param name="collider">The collider component used to get the adapter.</param>
        /// <returns>An implementation of <see cref="Abstract2DColliderAdapter{C}"/>.</returns>
        public static AbstractColliderAdapter GetAdapter2D(Collider2D collider)
        {
#if UNITY_2021_2_OR_NEWER
            AbstractColliderAdapter adapter = collider switch
            {
                BoxCollider2D => collider.gameObject.AddComponent<BoxCollider2DAdapter>(),
                CapsuleCollider2D => collider.gameObject.AddComponent<CapsuleCollider2DAdapter>(),
                CircleCollider2D => collider.gameObject.AddComponent<CircleCollider2DAdapter>(),
                CompositeCollider2D => collider.gameObject.AddComponent<CompositeCollider2DAdapter>(),
                _ => throw new System.NotImplementedException($"{collider.GetType()} does not have an adapter."),
            };
            return adapter;
#else
            switch (collider)
            {
                case BoxCollider2D _: return collider.gameObject.AddComponent<BoxCollider2DAdapter>();
                case CapsuleCollider2D _: return collider.gameObject.AddComponent<CapsuleCollider2DAdapter>();
                case CircleCollider2D _: return collider.gameObject.AddComponent<CircleCollider2DAdapter>();
                case CompositeCollider2D _: return collider.gameObject.AddComponent<CompositeCollider2DAdapter>();
                default: throw new System.NotImplementedException($"{collider.GetType()} does not have an adapter.");
            }
#endif
        }

        /// <summary>
        /// Gets the best implementation of <see cref="AbstractColliderAdapter"/> 
        /// as a 3D Collider Adapter based on given GameObject.
        /// </summary>
        /// <param name="collider">The collider component used to get the adapter.</param>
        /// <returns>An implementation of <see cref="Abstract3DColliderAdapter{C}"/>.</returns>
        public static AbstractColliderAdapter GetAdapter3D(Collider collider)
        {
#if UNITY_2021_2_OR_NEWER
            AbstractColliderAdapter adapter = collider switch
            {
                BoxCollider => collider.gameObject.AddComponent<BoxCollider3DAdapter>(),
                CapsuleCollider => collider.gameObject.AddComponent<CapsuleCollider3DAdapter>(),
                SphereCollider => collider.gameObject.AddComponent<SphereCollider3DAdapter>(),
                _ => throw new System.NotImplementedException($"{collider.GetType()} does not have an adapter."),
            };
            return adapter;
#else
            switch (collider)
            {
                case BoxCollider _: return collider.gameObject.AddComponent<BoxCollider3DAdapter>();
                case CapsuleCollider _: return collider.gameObject.AddComponent<CapsuleCollider3DAdapter>();
                case SphereCollider _: return collider.gameObject.AddComponent<SphereCollider3DAdapter>();
                default: throw new System.NotImplementedException($"{collider.GetType()} does not have an adapter.");
            }
#endif
        }

        /// <summary>
        /// Adds an implementation of <see cref="AbstractColliderAdapter"/> on the given GameObject.
        /// <para>A message will be displayed for the user to choose which Adapter to use if no Collider is found.</para>
        /// <para><b>This function should only be used on Editor time</b>, like MonoBehaviour.Reset()</para>
        /// </summary>
        /// <param name="gameObject">A GameObject to add a 2D Collider Adapter implementation.</param>
        /// <returns>An implementation of <see cref="Abstract2DColliderAdapter{C}"/>.</returns>
        public static AbstractColliderAdapter AddAdapter2D(GameObject gameObject)
        {
            var adapter = gameObject.GetComponent<AbstractColliderAdapter>();
#if UNITY_EDITOR
            if (adapter == null && CanDisplayEditorDialog())
            {
                var colliders = new System.Type[3]
                {
                    typeof(BoxCollider2DAdapter),
                    typeof(CircleCollider2DAdapter),
                    typeof(CapsuleCollider2DAdapter)
                };
                var message = $"No 2D Collider Adapter was found on '{gameObject.name}'.\nWhich one do you want?";
                var index = UnityEditor.EditorUtility.DisplayDialogComplex("Attention", message,
                    colliders[0].Name,
                    colliders[1].Name,
                    colliders[2].Name
                );

                adapter = gameObject.AddComponent(colliders[index]) as AbstractColliderAdapter;
            }
#endif
            return adapter;
        }

        /// <summary>
        /// Adds an implementation of <see cref="AbstractColliderAdapter"/> on the given GameObject.
        /// <para>A message will be displayed for the user to choose which Adapter to use if no Collider is found.</para>
        /// <para><b>This function should only be used on Editor time</b>, like MonoBehaviour.Reset()</para>
        /// </summary>
        /// <param name="gameObject">A GameObject to add a 3D Collider Adapter implementation.</param>
        /// <returns>An implementation of <see cref="Abstract3DColliderAdapter{C}"/>.</returns>
        public static AbstractColliderAdapter AddAdapter3D(GameObject gameObject)
        {
            var adapter = gameObject.GetComponent<AbstractColliderAdapter>();
#if UNITY_EDITOR
            if (adapter == null && CanDisplayEditorDialog())
            {
                var colliders = new System.Type[3]
                {
                    typeof(BoxCollider3DAdapter),
                    typeof(SphereCollider3DAdapter),
                    typeof(CapsuleCollider3DAdapter)
                };
                var message = $"No 3D Collider Adapter was found on '{gameObject.name}'.\nWhich one do you want?";
                var index = UnityEditor.EditorUtility.DisplayDialogComplex("Attention", message,
                    colliders[0].Name,
                    colliders[1].Name,
                    colliders[2].Name
                );

                adapter = gameObject.AddComponent(colliders[index]) as AbstractColliderAdapter;
            }
#endif
            return adapter;
        }

        /// <summary>
        /// Whether can display editor dialogs.
        /// </summary>
        /// <returns>True if can display editor dialogs. False otherwise.</returns>
        internal static bool CanDisplayEditorDialog() => Application.isEditor && !Application.isBatchMode;
    }
}