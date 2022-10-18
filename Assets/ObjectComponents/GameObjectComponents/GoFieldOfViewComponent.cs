

using UnityEngine;
using Zenject;

namespace ObjectComponents
{
    public class GoFieldOfViewComponent : GameObjectComponent
    {
        private const float FOV = 90f;
        private const int RAY_COUNT = 50;
        private float angle { get; set; }
        private Vector3 origin { get; set; }
        private float viewDistance { get; set; }
        private Mesh mesh { get; set; }
        private MonoBaseObject baseObject { get; set; }

        [Inject]
        public void Construct(MonoBaseObject _baseObj)
        {
            this.mesh = new Mesh();
            this.GetComponent<MeshFilter>().mesh = mesh;
            this.viewDistance = .5f;
            this.baseObject = _baseObj;
        }

        private void Update()
        {
            this.angle = 0f;
            this.origin = baseObject.transform.position;
            float angleIncrease = FOV / RAY_COUNT;
            Vector3[] vertices = new Vector3[RAY_COUNT + 1 + 1];
            Vector2[] uv = new Vector2[vertices.Length];
            int[] triangles = new int[RAY_COUNT * 3];

            vertices[0] = this.origin;
            int vertextIndex = 1;
            int triangleIndex = 0;
            for (int i = 0; i <= RAY_COUNT; i++)
            {
                Vector3 vertex;
                RaycastHit2D raycastHit2d = Physics2D.Raycast(this.origin, this.GetVectorFromAngle(this.angle), this.viewDistance);
                if (raycastHit2d.collider == null)
                {
                    vertex = this.origin + this.GetVectorFromAngle(this.angle) * this.viewDistance;
                }
                else
                {
                    vertex = raycastHit2d.point;
                }
                vertices[vertextIndex] = vertex;

                if (i > 0)
                {
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 1] = vertextIndex - 1;
                    triangles[triangleIndex + 2] = vertextIndex;
                    triangleIndex += 3;
                }
                vertextIndex++;
                this.angle -= angleIncrease;
            }

            this.mesh.vertices = vertices;
            this.mesh.uv = uv;
            this.mesh.triangles = triangles;
        }

        public Vector3 GetVectorFromAngle(float angle)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public class Factory : PlaceholderFactory<MonoBaseObject, GoFieldOfViewComponent>
        {
        }
    }
}
