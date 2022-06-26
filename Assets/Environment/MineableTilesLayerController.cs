using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class MineableTilesLayerController : MonoBehaviour
{
    public Tilemap tilemap;
    public MineableHunk hunk;

    // Start is called before the first frame update
    void Start()
    {
        print(this.tilemap.size);
        this.tilemap = GetComponent<Tilemap>();
        for (int x = 0; x < this.tilemap.size.x; x++)
        {
            for (int y = 0; y < this.tilemap.size.y; y++)
            {
                    MineableHunk obj = Instantiate(hunk, this.tilemap.CellToLocal(new Vector3Int(x,y,0)), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
