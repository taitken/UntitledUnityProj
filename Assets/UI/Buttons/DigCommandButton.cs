using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class DigCommandButton : HiveBaseButton
    {
        public Button buttonComponent;

        // Start is called before the first frame update
        void Start()
        {
            this.buttonComponent = GetComponent<Button>();
            this.buttonComponent.onClick.AddListener(ActivateDigMode);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ActivateDigMode()
        {
            Debug.Log("Hi Uwu");

        }
    }
}