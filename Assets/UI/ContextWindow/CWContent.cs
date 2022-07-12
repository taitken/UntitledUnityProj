using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CWContent : MonoBehaviour2
    {
        public void setText(string newText)
        {
            this.GetComponent<TextMeshProUGUI>().SetText(newText);
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
