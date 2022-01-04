using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Extension class for UnityEngine Collider class.
    /// </summary>
    public static class Collider3DExtension
    {
        private static readonly Collider[] buffer = new Collider[1];

        #region Casts Extensions
        /// <summary>
        /// Casts a box against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="filter">A set of parameters for filtering the cast results.</param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <returns>Whether the boxcast hits any collider in the Scene.</returns>
        public static bool Cast(this BoxCollider collider, CastFilter3D filter, out RaycastHit hit) =>
            Cast(collider, filter.Offset, filter.Direction, filter.Distance,
                filter.Collisions, out hit, filter.Skin, filter.Draw);

        /// <summary>
        /// Casts a box against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="offset"><inheritdoc cref="CastFilter3D.Offset"/></param>
        /// <param name="direction"><inheritdoc cref="CastFilter3D.Direction"/></param>
        /// <param name="distance"><inheritdoc cref="CastFilter3D.Distance"/></param>
        /// <param name="collisions"><inheritdoc cref="CastFilter3D.Collisions"/></param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <param name="skin"><inheritdoc cref="CastFilter3D.Skin"/></param>
        /// <param name="draw"><inheritdoc cref="CastFilter3D.Draw"/></param>
        /// <returns>Whether the boxcast hits any collider in the Scene.</returns>
        public static bool Cast(this BoxCollider collider,
            Vector3 offset, Vector3 direction,
            float distance, int collisions, out RaycastHit hit,
            float skin = 0f, bool draw = false)
        {
            var bounds = collider.bounds;
            var origin = collider.transform.position + offset;
            var orientation = collider.transform.rotation;
            var size = bounds.size - Vector3.one * skin;
            var halfSize = size * 0.5f;
            var wasHit = Physics.BoxCast(origin, halfSize, direction, out hit, orientation, distance, collisions);
            if (draw) hit.DrawBoxCast(origin, halfSize, direction, orientation, distance);
            return wasHit;
        }

        /// <summary>
        /// Casts a sphere against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="filter">A set of parameters for filtering the cast results.</param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <returns>Whether the sphere-cast hits any collider in the Scene.</returns>
        public static bool Cast(this SphereCollider collider, CastFilter3D filter, out RaycastHit hit) =>
            Cast(collider, filter.Offset, filter.Direction, filter.Distance,
                filter.Collisions, out hit, filter.Skin, filter.Draw);

        /// <summary>
        /// Casts a sphere against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="offset"><inheritdoc cref="CastFilter3D.Offset"/></param>
        /// <param name="direction"><inheritdoc cref="CastFilter3D.Direction"/></param>
        /// <param name="distance"><inheritdoc cref="CastFilter3D.Distance"/></param>
        /// <param name="collisions"><inheritdoc cref="CastFilter3D.Collisions"/></param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <param name="skin"><inheritdoc cref="CastFilter3D.Skin"/></param>
        /// <param name="draw"><inheritdoc cref="CastFilter3D.Draw"/></param>
        /// <returns>Whether the sphere-cast hits any collider in the Scene.</returns>
        public static bool Cast(this SphereCollider collider,
            Vector3 offset, Vector3 direction,
            float distance, int collisions, out RaycastHit hit,
            float skin = 0f, bool draw = false)
        {
            var origin = collider.transform.position + offset;
            var radius = collider.radius - skin;
            var wasHit = Physics.SphereCast(origin, radius, direction, out hit, distance, collisions);
            if (draw) hit.DrawSphereCast(origin, radius, direction, distance);
            return wasHit;
        }
        #endregion

        #region Overlapping Extensions

        #region BoxCollider
        /// <summary>
        /// Checks whether overlapping other colliders.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="mask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="interaction">Should it hit Triggers?</param>
        /// <returns>True if overlapping other colliders. False otherwise.</returns>
        public static bool IsOverlapping(this BoxCollider collider, int mask,
            QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            var bounds = collider.bounds;
            var wasEnabled = collider.enabled;

            collider.enabled = false; // disabling so it doesn't collide with itself.
            var wasHit = Physics.CheckBox(bounds.center, bounds.extents, collider.transform.rotation, mask, interaction);
            collider.enabled = wasEnabled;

            return wasHit;
        }

        /// <summary>
        /// Checks whether overlapping other collider and store it into the given colliding param.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="mask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="colliding">The output argument that will contain the collider or null if not colliding.</param>
        /// <param name="interaction">Should it hit Triggers?</param>
        /// <returns>True if overlapping other collider. False otherwise.</returns>
        public static bool IsOverlapping(this BoxCollider collider, int mask, out Collider colliding,
            QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            var bounds = collider.bounds;
            var wasEnabled = collider.enabled;

            collider.enabled = false; // disabling so it doesn't collide with itself.
            int totalHits = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, buffer,
                collider.transform.rotation, mask, interaction);
            collider.enabled = wasEnabled;

            var wasHit = totalHits > 0;
            colliding = wasHit ? buffer[0] : null;

            return wasHit;
        }

        /// <summary>
        /// Finds all overlapping colliders and store them into the given colliders buffer.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="mask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="colliders">The colliders buffer to store the results in.</param>
        /// <param name="interaction">Should it hit Triggers?</param>
        /// <returns>The amount of colliders stored in colliders buffer.</returns>
        public static int IsOverlapping(this BoxCollider collider, int mask, Collider[] colliders,
            QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            var bounds = collider.bounds;
            var wasEnabled = collider.enabled;

            collider.enabled = false; // disabling so it doesn't collide with itself.
            int totalHits = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, colliders,
                collider.transform.rotation, mask, interaction);
            collider.enabled = wasEnabled;

            return totalHits;
        }
        #endregion

        #region SphereCollider
        /// <summary>
        /// Checks whether overlapping other colliders.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="mask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="interaction">Should it hit Triggers?</param>
        /// <returns>True if overlapping other colliders. False otherwise.</returns>
        public static bool IsOverlapping(this SphereCollider collider, int mask,
            QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            var wasEnabled = collider.enabled;
            collider.enabled = false; // disabling so it doesn't collide with itself.
            var wasHit = Physics.CheckSphere(collider.transform.position, collider.radius, mask, interaction);
            collider.enabled = wasEnabled;

            return wasHit;
        }

        /// <summary>
        /// Checks whether overlapping other collider and store it into the given colliding param.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="mask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="colliding">The output argument that will contain the collider or null if not colliding.</param>
        /// <param name="interaction">Should it hit Triggers?</param>
        /// <returns>True if overlapping other collider. False otherwise.</returns>
        public static bool IsOverlapping(this SphereCollider collider, int mask, out Collider colliding,
            QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            var wasEnabled = collider.enabled;
            collider.enabled = false; // disabling so it doesn't collide with itself.
            int totalHits = Physics.OverlapSphereNonAlloc(collider.transform.position, collider.radius,
                buffer, mask, interaction);
            collider.enabled = wasEnabled;

            var wasHit = totalHits > 0;
            colliding = wasHit ? buffer[0] : null;

            return wasHit;
        }

        /// <summary>
        /// Finds all overlapping colliders and store them into the given colliders buffer.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="mask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="colliders">The colliders buffer to store the results in.</param>
        /// <param name="interaction">Should it hit Triggers?</param>
        /// <returns>The amount of colliders stored in colliders buffer.</returns>
        public static int IsOverlapping(this SphereCollider collider, int mask, Collider[] colliders,
            QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            var wasEnabled = collider.enabled;
            collider.enabled = false; // disabling so it doesn't collide with itself.
            int totalHits = Physics.OverlapSphereNonAlloc(collider.transform.position, collider.radius,
                colliders, mask, interaction);
            collider.enabled = wasEnabled;

            return totalHits;
        }
        #endregion

        #endregion
    }
}