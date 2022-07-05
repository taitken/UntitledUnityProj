using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Characters.Utils
{
    public class CharacterPathLine : MonoBehaviour2
    {

        private LineRenderer lineRenderer;
        private IList<Vector3> initalPath;

        [Inject]
        public void Construct(IList<Vector3> _path)
        {
            this.initalPath = _path;
        }
        // Start is called before the first frame update
        void Start()
        {
            this.lineRenderer = GetComponent<LineRenderer>();
            this.UpdateLine(this.initalPath);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateLine(IList<Vector3> newPath)
        {
            Vector3[] positions = new Vector3[newPath.Count];
            newPath.CopyTo(positions, 0);
            this.lineRenderer.positionCount = newPath.Count;
            this.lineRenderer.SetPositions(positions);
        }

        public void SetUpLine(Transform[] points)
        {
        }

        public class Factory : PlaceholderFactory<IList<Vector3>, CharacterPathLine>
        {
        }
    }
}
