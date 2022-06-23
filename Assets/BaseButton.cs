using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BaseButton : MonoBehaviour
{

    Button button;
    // Start is called before the first frame update
    void Start()
    {
        this.button = GetComponent<Button>();
        this.button.GetComponentInChildren<Text>().text = "test";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
