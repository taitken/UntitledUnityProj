using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class HiveBaseButton : MonoBehaviour2
    {
        public Button buttonComponent;

        void OnAwake()
        {
            this.buttonComponent = GetComponent<Button>();
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

