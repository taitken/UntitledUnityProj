

using System.Collections.Generic;
using Environment;
using Environment.Models;
using GameControllers.Services;
using UnityEngine;
using Zenject;

namespace ObjectComponents
{
    public class GoFieldOfViewComponent : GameObjectComponent
    {
        private const float FOV = 360f;
        private const int RAY_COUNT = 16;
        private float angle { get; set; }
        private Vector3 origin { get; set; }
        private float viewDistance { get; set; }
        private Mesh mesh { get; set; }
        private MonoBaseObject baseObject { get; set; }
        private IEnvironmentService envService { get; set; }

        [Inject]
        public void Construct(MonoBaseObject _baseObj,
                                IEnvironmentService _envService)
        {
            this.mesh = new Mesh();
            this.GetComponent<MeshFilter>().mesh = mesh;
            this.viewDistance = .5f;
            this.baseObject = _baseObj;
            this.envService = _envService;
        }

        private void Update()
        {
            List<FogModel> hitFogModels = new List<FogModel>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("FogLayer"));
            this.angle = 0f;
            this.origin = baseObject.transform.position;
            float angleIncrease = FOV / RAY_COUNT;
            for (int i = 0; i <= RAY_COUNT; i++)
            {
                List<RaycastHit2D> raycastHit2d = new List<RaycastHit2D>();
                Physics2D.Raycast(this.origin, this.GetVectorFromAngle(this.angle), filter, raycastHit2d, this.viewDistance);
                raycastHit2d.ForEach(hit =>
                {
                    FogObject hitFogObject = hit.collider.gameObject.GetComponent<FogObject>();
                    if (hitFogObject != null)
                    {
                        hitFogModels.Add(hitFogObject.fogModel);
                    }
                });
                this.angle -= angleIncrease;
            }
            if (hitFogModels.Count > 0)
            {
                FogModel[,] finalFodModels = this.envService.GetFogObservable().Get();
                for (int i = 0; i < finalFodModels.GetLength(0); i++)
                {
                    for (int ii = 0; ii < finalFodModels.GetLength(1); ii++)
                    {
                        if (hitFogModels.Any(hitModel => { return hitModel == finalFodModels[i, ii]; }))
                        {
                            finalFodModels[i, ii] = null;
                        }
                    }
                }
                this.envService.GetFogObservable().Set(finalFodModels);
            }
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
