using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class MineableHunk : MonoBehaviour2
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        print("clicked");
    }

    public override void OnClickedByUser()
    {
        Destroy(gameObject);
    }
    
}
