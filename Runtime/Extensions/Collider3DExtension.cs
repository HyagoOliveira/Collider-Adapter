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
        /// Casts a capsule against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="filter">A set of parameters for filtering the cast results.</param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <returns>Whether the capsule-cast hits any collider in the Scene.</returns>
        public static bool Cast(this CapsuleCollider collider, CastFilter3D filter, out RaycastHit hit) =>
            Cast(collider, filter.Offset, filter.Direction, filter.Distance,
                filter.Collisions, out hit, filter.Skin, filter.Draw);

        /// <summary>
        /// Casts a capsule against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="offset"><inheritdoc cref="CastFilter3D.offset"/></param>
        /// <param name="direction"><inheritdoc cref="CastFilter3D.direction"/></param>
        /// <param name="distance"><inheritdoc cref="CastFilter3D.distance"/></param>
        /// <param name="collisions"><inheritdoc cref="CastFilter3D.collisions"/></param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <param name="skin"><inheritdoc cref="CastFilter3D.skin"/></param>
        /// <param name="draw"><inheritdoc cref="CastFilter3D.draw"/></param>
        /// <returns>Whether the capsule-cast hits any collider in the Scene.</returns>
        public static bool Cast(this CapsuleCollider collider,
            Vector3 offset, Vector3 direction,
            float distance, int collisions, out RaycastHit hit,
            float skin = 0f, bool draw = false)
        {
            var origin = collider.transform.position + offset;
            var orientation = collider.transform.rotation;
            var radius = collider.radius - skin;

            var axisDirection = Vector3.zero;
            if (collider.direction == 0) axisDirection = Vector3.right;
            else if (collider.direction == 1) axisDirection = Vector3.up;
            else if (collider.direction == 2) axisDirection = Vector3.forward;

            var halfHeight = collider.height * 0.5F;
            var axisDistance = axisDirection * (halfHeight - radius);
            var point1 = origin + axisDistance;
            var point2 = origin - axisDistance;
            var wasHit = Physics.CapsuleCast(point1, point2, radius, direction, out hit, distance, collisions);

            if (draw)
            {
                var diameter = radius * 2F;
                hit.DrawCapsuleCast(origin, axisDistance, axisDirection,
                    collider.transform.right, direction, orientation, distance, diameter);
            }
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

        #region CapsuleCollider
        /// <summary>
        /// Checks whether overlapping other colliders.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="mask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="interaction">Should it hit Triggers?</param>
        /// <returns>True if overlapping other colliders. False otherwise.</returns>
        public static bool IsOverlapping(this CapsuleCollider collider, int mask,
            QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            (Vector3 point0, Vector3 point1) = GetPoints(collider);

            var wasEnabled = collider.enabled;
            collider.enabled = false; // disabling so it doesn't collide with itself.
            var wasHit = Physics.CheckCapsule(point0, point1, collider.radius, mask, interaction);
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
        public static bool IsOverlapping(this CapsuleCollider collider, int mask, out Collider colliding,
            QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            (Vector3 point0, Vector3 point1) = GetPoints(collider);

            var wasEnabled = collider.enabled;
            collider.enabled = false; // disabling so it doesn't collide with itself.
            int totalHits = Physics.OverlapCapsuleNonAlloc(point0, point1, collider.radius, buffer, mask, interaction);
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
        public static int IsOverlapping(this CapsuleCollider collider, int mask, Collider[] colliders,
            QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            (Vector3 point0, Vector3 point1) = GetPoints(collider);

            var wasEnabled = collider.enabled;
            collider.enabled = false; // disabling so it doesn't collide with itself.
            int totalHits = Physics.OverlapCapsuleNonAlloc(point0, point1, collider.radius, colliders, mask, interaction);
            collider.enabled = wasEnabled;

            return totalHits;
        }

        /// <summary>
        /// Gets the two points representing the capsule spheres at its ends. 
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>A Vector3 tuple representing the capsule spheres.</returns>
        public static (Vector3, Vector3) GetPoints(this CapsuleCollider collider)
        {
            var direction = new Vector3 { [collider.direction] = 1 };
            var offset = collider.height * 0.5F - collider.radius;
            var localPoint0 = collider.center - direction * offset;
            var localPoint1 = collider.center + direction * offset;
            var point0 = collider.transform.TransformPoint(localPoint0);
            var point1 = collider.transform.TransformPoint(localPoint1);
            return (point0, point1);
        }
        #endregion

        #endregion
    }
}