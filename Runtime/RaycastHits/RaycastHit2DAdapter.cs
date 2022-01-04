namespace UnityEngine
{
    /// <summary>
    /// Adapter class for <see cref="RaycastHit2D"/>.
    /// </summary>
    public sealed class RaycastHit2DAdapter : IRaycastHit
    {
        public Collider2D Collider => raycastHit.collider;

        public Rigidbody2D Rigidbody => raycastHit.rigidbody;

        public Transform Transform => raycastHit.transform;

        public Vector3 Point
        {
            get => raycastHit.point;
            set => raycastHit.point = value;
        }

        public Vector3 Normal
        {
            get => raycastHit.normal;
            set => raycastHit.normal = value;
        }

        public float Distance
        {
            get => raycastHit.distance;
            set => raycastHit.distance = value;
        }

        public Vector2 Centroid
        {
            get => raycastHit.centroid;
            set => raycastHit.centroid = value;
        }

        public float Fraction
        {
            get => raycastHit.fraction;
            set => raycastHit.fraction = value;
        }

        private RaycastHit2D raycastHit;

        public RaycastHit2DAdapter(RaycastHit2D raycastHit) => this.raycastHit = raycastHit;

        public bool HasCollider() => Collider != null;
    }
}