using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Extension class for <see cref="Collider2D"/>.
    /// </summary>
    public static class Collider2DExtension
    {
        /// <summary>
        /// Casts a box against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="filter">A set of parameters for filtering the cast results.</param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <returns>Whether the boxcast hits any collider in the Scene.</returns>
        public static bool Cast(this BoxCollider2D collider, CastFilter2D filter, out RaycastHit2D hit) =>
            Cast(collider, filter.Offset, filter.Direction, filter.Distance, filter.Collisions,
                out hit, filter.Angle, filter.MinDepth, filter.MaxDepth, filter.Skin, filter.Draw);

        /// <summary>
        /// Casts a box against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="offset"><inheritdoc cref="CastFilter2D.Offset"/></param>
        /// <param name="direction"><inheritdoc cref="CastFilter2D.Direction"/></param>
        /// <param name="distance"><inheritdoc cref="CastFilter2D.Distance"/></param>
        /// <param name="collisions"><inheritdoc cref="CastFilter2D.Collisions"/></param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <param name="angle"><inheritdoc cref="CastFilter2D.Angle"/></param>
        /// <param name="minDepth"><inheritdoc cref="CastFilter2D.MinDepth"/></param>
        /// <param name="maxDepth"><inheritdoc cref="CastFilter2D.MaxDepth"/></param>
        /// <param name="skin"><inheritdoc cref="CastFilter2D.Skin"/></param>
        /// <param name="draw"><inheritdoc cref="CastFilter2D.Draw"/></param>
        /// <returns>Whether the boxcast hits any collider in the Scene.</returns>
        public static bool Cast(this BoxCollider2D collider,
            Vector3 offset, Vector2 direction, float distance, int collisions,
            out RaycastHit2D hit, float angle = 0f, float minDepth = 0f, float maxDepth = 0f,
            float skin = 0f, bool draw = false)
        {
            var bounds = collider.bounds;
            var origin = collider.transform.position + offset;
            var size = bounds.size - Vector3.one * skin;

            hit = Physics2D.BoxCast(origin, size, angle, direction, distance, collisions, minDepth, maxDepth);
            if (draw) hit.DrawBoxCast(origin, size, angle, direction, distance);
            return hit;
        }

        /// <summary>
        /// Casts a circle against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="filter">A set of parameters for filtering the cast results.</param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <returns>Whether the circle-cast hits any collider in the Scene.</returns>
        public static bool Cast(this CircleCollider2D collider, CastFilter2D filter, out RaycastHit2D hit) =>
            Cast(collider, filter.Offset, filter.Direction, filter.Distance, filter.Collisions,
                out hit, filter.MinDepth, filter.MaxDepth, filter.Skin, filter.Draw);

        /// <summary>
        /// Casts a circle against Colliders in the Scene, 
        /// gathering information about the first Collider to contact with.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="offset"><inheritdoc cref="CastFilter2D.Offset"/></param>
        /// <param name="direction"><inheritdoc cref="CastFilter2D.Direction"/></param>
        /// <param name="distance"><inheritdoc cref="CastFilter2D.Distance"/></param>
        /// <param name="collisions"><inheritdoc cref="CastFilter2D.Collisions"/></param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <param name="minDepth"><inheritdoc cref="CastFilter2D.MinDepth"/></param>
        /// <param name="maxDepth"><inheritdoc cref="CastFilter2D.MaxDepth"/></param>
        /// <param name="skin"><inheritdoc cref="CastFilter2D.Skin"/></param>
        /// <param name="draw"><inheritdoc cref="CastFilter2D.Draw"/></param>
        /// <returns>Whether the circle-cast hits any collider in the Scene.</returns>
        public static bool Cast(this CircleCollider2D collider,
            Vector3 offset, Vector2 direction, float distance, int collisions,
            out RaycastHit2D hit, float minDepth = 0f, float maxDepth = 0f,
            float skin = 0f, bool draw = false)
        {
            var origin = collider.transform.position + offset;
            var radius = collider.radius - skin;

            hit = Physics2D.CircleCast(origin, radius, direction, distance, collisions, minDepth, maxDepth);
            if (draw) hit.DrawCircleCast(origin, radius, direction, distance);
            return hit;
        }

        /// <summary>
        /// Checks whether the Collider is touching any Colliders on the specified layerMask or not.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="layerMask">Any Colliders on any of these layers count as touching.</param>
        /// <returns>Whether the Collider is touching any Colliders on the specified layerMask or not.</returns>
        public static bool IsColliding(this Collider2D collider, int layerMask) =>
            Physics2D.IsTouchingLayers(collider, layerMask);
    }
}