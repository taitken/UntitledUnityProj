using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ItemMassText : MonoBehaviour2
    {
        public void SetText(string newText)
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