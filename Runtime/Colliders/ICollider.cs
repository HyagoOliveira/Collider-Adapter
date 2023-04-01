using UnityEngine;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Interface used on objects able to be a Collider.
    /// </summary>
    public interface ICollider
    {
        /// <summary>
        /// <inheritdoc cref="Collider.enabled"/>
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// <inheritdoc cref="Collider.isTrigger"/>
        /// </summary>
        bool IsTrigger { get; set; }

        /// <summary>
        /// The collider center.
        /// </summary>
        Vector3 Center { get; }

        /// <summary>
        /// The collider offset.
        /// </summary>
        Vector3 Offset { get; set; }

        /// <summary>
        /// The collider size.
        /// </summary>
        Vector3 Size { get; set; }

        /// <summary>
        /// The collider half size, i.e. its extends.
        /// </summary>
        Vector3 HalfSize { get; }

        /// <summary>
        /// <inheritdoc cref="Collider.bounds"/>
        /// </summary>
        Bounds Bounds { get; }

        /// <summary>
        /// <inheritdoc cref="Collider.ClosestPoint(Vector3)"/>
        /// </summary>
        Vector3 ClosestPoint(Vector3 position);

        /// <summary>
        /// Checks if colliding based on the given layerMask.
        /// </summary>
        /// <param name="layerMask">Filter to check objects only on specific layers.</param>
        /// <returns>Whether is colliding.</returns>
        bool IsColliding(int layerMask);

        /// <summary>
        /// Checks if colliding with any Component using the given type and layerMask.
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve.</typeparam>
        /// <param name="layerMask">Filter to check objects only on specific layers.</param>
        /// <param name="component">The output argument that will contain the component or default one.</param>
        /// <returns>Whether was a collision.</returns>
        bool TryToGetCollidingComponent<T>(int layerMask, out T component);

        /// <summary>
        /// Checks if colliding with any Component using the given type and layerMask.
        /// </summary>
        /// <typeparam name="T">The type of the components to retrieve.</typeparam>
        /// <param name="layerMask">Filter to check objects only on specific layers.</param>
        /// <param name="components">The buffer to store the components in.</param>
        /// <returns>The amount of components stored in the buffer.</returns>
        int TryToGetCollidingComponents<T>(int layerMask, T[] components);

        /// <summary>
        /// Casts this Collider against other Colliders in the Scene, 
        /// gathering information about the first one to contact with.
        /// </summary>
        /// <param name="offset">The offset position from the Collider origin.</param>
        /// <param name="direction">The cast direction.</param>
        /// <param name="hit">The cast information about the first detected object.</param>
        /// <param name="maxDistance">The maximum cast distance.</param>
        /// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="draw">Draws the cast if enabled.</param>
        /// <returns>Whether the cast hits any Collider in the Scene.</returns>
        bool Cast(Vector3 direction, out IRaycastHit hit, float maxDistance, int layerMask, bool draw = false);

        /// <summary>
        /// Casts multiples rays along the given point1 and point2 against Colliders in the Scene, 
        /// gathering information about the closest Collider to contact with.
        /// <para>Only detect collisions on hits where its angle is above the given angleLimit.</para>
        /// </summary>
        /// <param name="point1">The first point to cast the rays.</param>
        /// <param name="point2">The second point to cast the rays.</param>
        /// <param name="direction">The direction to cast the rays.</param>
        /// <param name="closestHit">The cast information about the closest detected Collider.</param>
        /// <param name="distance">The maximum distance to cast the rays.</param>
        /// <param name="mask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="raysCount">The number of rays to cast.</param>
        /// <param name="draw">Draws the raycasts if enabled.</param>
        /// <returns>Whether the raycasts hit any Collider along the given points.</returns>
        bool Raycasts(Vector3 point1, Vector3 point2, Vector3 direction,
            out IRaycastHit closestHit, float distance, int mask,
            int raysCount = 2, bool draw = false);

        /// <summary>
        /// Returns the biggest axis of <see cref="Size"/>.
        /// </summary>
        /// <returns>The biggest X, Y or Z on <see cref="Size"/>.</returns>
        float GetBiggestSizeAxis();
    }
}