using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;

public class ActionController : MonoBehaviour
{
    public Observable<string> testobs {get;set;}
    // Start is called before the first frame update
    void Start()
    {
        this.testobs = new Observable<string>("hello");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
