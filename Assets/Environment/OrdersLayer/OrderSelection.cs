using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class OrderSelection : MonoBehaviour2
    {
        public Vector3Int position { get; set; }

        [Inject]
        public void Construct(Vector3Int _cellPosition, Vector3 _localPosition)
        {
            this.position = _cellPosition;
            this.transform.position = _localPosition;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public class Factory : PlaceholderFactory<Vector3Int,Vector3, OrderSelection>
        {
        }
    }
}