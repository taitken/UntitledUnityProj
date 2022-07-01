using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Environment
{
    public class GroundLayer : MonoBehaviour2
    {
        private Tilemap tilemap;
        // Start is called before the first frame update
        void Start()
        {
            this.tilemap = this.GetComponent<Tilemap>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
