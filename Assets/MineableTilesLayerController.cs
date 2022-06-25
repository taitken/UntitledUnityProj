using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MineableTilesLayerController : MonoBehaviour
{
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        this.tilemap = GetComponent<Tilemap>();
        for (int i = 0; i < this.tilemap.size.x; i++)
        {
            for (int ii = 0; ii < this.tilemap.size.y; ii++)
            {
                TileBase tile = this.tilemap.GetTile(new Vector3Int(i, ii, 0));
                if(tile)
                {
                    
                    //print(tile.GetType().FullName.ToString());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
