﻿using UnityEngine;
using ActionCode.Shapes;

namespace ActionCode.ColliderAdapter
{
    /// <summary>
    /// Extension class for <see cref="RaycastHit2D"/>.
    /// </summary>
    public static class RaycastHit2DExtension
    {
        /// <summary>
        /// Draws a 2D Raycast hit using the given params.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="origin">The Raycast origin.</param>
        /// <param name="direction">The Raycast direction.</param>
        /// <param name="distance">The Raycast distance.</param>
        public static void Draw(this RaycastHit2D hit, Vector2 origin, Vector2 direction, float distance)
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
        /// Draws a 2D BoxCast hit using the given params.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="origin">The Raycast origin.</param>
        /// <param name="size">The Raycast size.</param>
        /// <param name="angle">The box angle to draw.</param>
        /// <param name="direction">The Raycast direction.</param>
        /// <param name="distance">The Raycast distance.</param>
        public static void DrawBoxCast(this RaycastHit2D hit, Vector2 origin, Vector2 size, float angle,
            Vector2 direction, float distance)
        {
            var end = origin + direction * distance;
            var color = ExtensionConstants.COLLISION_OFF;
            var rotation = Quaternion.AngleAxis(angle, Vector3.back);

            if (hit.collider)
            {
                color = ExtensionConstants.COLLISION_ON;
                hit.point.Draw(color, ExtensionConstants.POINT_SIZE);
            }

            Debug.DrawLine(origin, end, color);
            ShapeDebug.DrawPlane(position: end, size, rotation, color);
        }

        /// <summary>
        /// Draws a 2D CapsuleCast hit using the given params.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="origin">The Raycast origin.</param>
        /// <param name="rightDirection">The capsule right direction.</param>
        /// <param name="rotation">The capsule rotation.</param>
        /// <param name="capsuleDirection">The capsule axis direction.</param>
        /// <param name="radius">The capsule radius.</param>
        /// <param name="height">The capsule height.</param>
        /// <param name="direction">The Raycast direction.</param>
        /// <param name="distance">The Raycast distance.</param>
        public static void DrawCapsuleCast(this RaycastHit2D hit, Vector2 origin, Vector2 rightDirection,
            Quaternion rotation, CapsuleDirection2D capsuleDirection, float radius, float height,
            Vector2 direction, float distance)
        {
            var end = origin + direction * distance;
            var color = ExtensionConstants.COLLISION_OFF;
            var horizontalCapsule = capsuleDirection == CapsuleDirection2D.Horizontal;
            var axisDirection = horizontalCapsule ?
                Vector3.right :
                Vector3.up;

            if (hit.collider)
            {
                color = ExtensionConstants.COLLISION_ON;
                hit.point.Draw(color, ExtensionConstants.POINT_SIZE);
            }

            Debug.DrawLine(origin, end, color);
            ShapeDebug.DrawCapsule(end, rightDirection, rotation, radius, height, axisDirection, color);
        }

        /// <summary>
        /// Draws a 2D CircleCast hit using the given params.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="origin">The Raycast origin.</param>
        /// <param name="radius">The circle radius.</param>
        /// <param name="direction">The Raycast direction.</param>
        /// <param name="distance">The Raycast distance.</param>
        public static void DrawCircleCast(this RaycastHit2D hit, Vector2 origin, float radius,
            Vector2 direction, float distance)
        {
            var end = origin + direction * distance;
            var color = ExtensionConstants.COLLISION_OFF;

            if (hit.collider)
            {
                color = ExtensionConstants.COLLISION_ON;
                hit.point.Draw(color, ExtensionConstants.POINT_SIZE);
            }

            Debug.DrawLine(origin, end, color);
            ShapeDebug.DrawCircle(
                position: end,
                normal: Vector3.forward,
                diameter: radius * 2f,
                color
            );
        }
    }
}