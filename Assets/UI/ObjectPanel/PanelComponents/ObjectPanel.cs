using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Models;
using UnityEngine;

public class ObjectPanel : MonoBehaviour2
{
    // Start is called before the first frame update
    public void Construct(PanelModel panelWindowModel)
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        texts.ForEach(text =>{
            if(text.tag == "UiHeader") text.SetText(panelWindowModel.title);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
