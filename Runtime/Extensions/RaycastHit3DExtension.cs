﻿using UnityEngine;
using ActionCode.Shapes;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Extension class for <see cref="RaycastHit"/>.
    /// </summary>
    public static class RaycastHit3DExtension
    {
        /// <summary>
        /// Draws a 3D Raycast hit using the given params.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="origin">The Raycast origin.</param>
        /// <param name="direction">The Raycast direction.</param>
        /// <param name="distance">The Raycast max distance.</param>
        public static void Draw(this RaycastHit hit, Vector3 origin, Vector3 direction, float distance)
        {
            var end = origin + direction * distance;
            var color = ExtensionConstants.COLLISION_OFF;

            if (hit.collider)
            {
                color = ExtensionConstants.COLLISION_ON;
                hit.point.Draw(color, ExtensionConstants.POINT_SIZE);
            }

            Debug.DrawLine(origin, end, color);
        }

        /// <summary>
        /// Draws a 3D BoxCast hit using the given params.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="center">The Boxcast center.</param>
        /// <param name="halfExtents">The Boxcast half extends.</param>
        /// <param name="direction">The Boxcast direction.</param>
        /// <param name="orientation">The BoxCast orientation.</param>
        /// <param name="distance">The Boxcast max distance.</param>
        public static void DrawBoxCast(this RaycastHit hit, Vector3 center, Vector3 halfExtents,
            Vector3 direction, Quaternion orientation, float distance)
        {
            var end = center + direction * distance;
            var color = ExtensionConstants.COLLISION_OFF;

            if (hit.collider)
            {
                color = ExtensionConstants.COLLISION_ON;
                hit.point.Draw(color, ExtensionConstants.POINT_SIZE);
            }

            Debug.DrawLine(center, end, color);
            ShapeDebug.DrawCuboid(end, halfExtents * 2f, orientation, color);
        }

        /// <summary>
        /// Draws a 3D CapsuleCast hit using the given params.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="center">The capsule-cast center.</param>
        /// <param name="axisDirection">The capsule axis direction.</param>
        /// <param name="direction">The capsule-cast direction.</param>
        /// <param name="orientation">The capsule-cast orientation.</param>
        /// <param name="distance">The capsule-cast max distance.</param>
        /// <param name="diameter">The diameter of the sphere, measured in the object's local space.</param>
        public static void DrawCapsuleCast(this RaycastHit hit, Vector3 center,
            Vector3 axisDistance, Vector3 axisDirection,
            Vector3 rightDirection, Vector3 direction,
            Quaternion orientation, float distance, float diameter)
        {
            var end = center + direction * distance;
            var color = ExtensionConstants.COLLISION_OFF;

            if (hit.collider)
            {
                color = ExtensionConstants.COLLISION_ON;
                hit.point.Draw(color, ExtensionConstants.POINT_SIZE);
            }

            Debug.DrawLine(center, end, color);
            ShapeDebug.DrawCapsule3D(end + axisDistance, end - axisDistance, axisDirection,
                rightDirection, orientation, radius: diameter * 0.5F, color);
        }

        /// <summary>
        /// Draws a SphereCast hit using the given params.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="origin">The Raycast origin.</param>
        /// <param name="radius">The sphere radius.</param>
        /// <param name="direction">The Raycast direction.</param>
        /// <param name="distance">The Raycast distance.</param>
        public static void DrawSphereCast(this RaycastHit hit, Vector3 origin, float radius,
            Vector3 direction, float distance)
        {
            var end = origin + direction * distance;
            var color = ExtensionConstants.COLLISION_OFF;

            if (hit.collider)
            {
                color = ExtensionConstants.COLLISION_ON;
                hit.point.Draw(color, ExtensionConstants.POINT_SIZE);
            }

            Debug.DrawLine(origin, end, color);
            ShapeDebug.DrawSphere(end, radius * 2f, color);
        }
    }
}