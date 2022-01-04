namespace UnityEngine
{
    /// <summary>
    /// Adapter class for <see cref="RaycastHit"/>.
    /// </summary>
    public sealed class RaycastHit3DAdapter : IRaycastHit
    {
        public Collider Collider => raycastHit.collider;

        public Rigidbody Rigidbody => raycastHit.rigidbody;

        public Transform Transform => raycastHit.transform;

        public int TriangleIndex => raycastHit.triangleIndex;

        public Vector2 TextureCoord => raycastHit.textureCoord;

        public Vector2 TextureCoord2 => raycastHit.textureCoord2;

        public ArticulationBody ArticulationBody => raycastHit.articulationBody;

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

        public Vector3 BarycentricCoordinate
        {
            get => raycastHit.barycentricCoordinate;
            set => raycastHit.barycentricCoordinate = value;
        }

        private RaycastHit raycastHit;

        public RaycastHit3DAdapter(RaycastHit raycastHit) => this.raycastHit = raycastHit;

        public bool HasCollider() => Collider != null;
    }
}