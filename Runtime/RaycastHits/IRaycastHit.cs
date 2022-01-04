namespace UnityEngine
{
    /// <summary>
    /// Interface used on objects able to be a Raycast hit.
    /// </summary>
    public interface IRaycastHit
    {
        /// <summary>
        /// <inheritdoc cref="RaycastHit.point"/>
        /// </summary>
        Vector3 Point { get; set; }

        /// <summary>
        /// <inheritdoc cref="RaycastHit.normal"/>
        /// </summary>
        Vector3 Normal { get; set; }

        /// <summary>
        /// <inheritdoc cref="RaycastHit.distance"/>
        /// </summary>
        float Distance { get; set; }

        /// <summary>
        /// <inheritdoc cref="RaycastHit.transform"/>
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        /// Whether it has hit a collider.
        /// </summary>
        /// <returns>True if it has hit a collider. False otherwise.</returns>
        bool HasCollider();
    }
}