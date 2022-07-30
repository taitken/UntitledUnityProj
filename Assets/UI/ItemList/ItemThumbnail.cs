using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemThumbnail : MonoBehaviour2
    {
        public void SetImage(Sprite sprite)
        {
            this.GetComponent<Image>().sprite = sprite;
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