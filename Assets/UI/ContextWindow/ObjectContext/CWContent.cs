using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CWContent : MonoBehaviour2
    {
        public void setText(IList<string> newText)
        {
            string newList = "";
            newText.ForEach((textItem, index) =>{
                newList = newList + "• " + textItem + (index < newText.Count ? "\n" : "");
            });
            this.GetComponent<TextMeshProUGUI>().text = newList;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
