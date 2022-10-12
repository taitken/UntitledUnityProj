using System;
using System.Collections;
using System.Collections.Generic;
using ObjectComponents;
using TMPro;
using UI.GenericComponents;
using UnityEngine;
using UtilityClasses;

namespace UI.Panel
{
    public class CharacterNeedsTab : BaseObjectTabContent
    {
        public TextMeshProUGUI textBox;
        public PlainProgressBar hungerBar;
        public BaseObjectModel objectModel;
        private ObjectNeedsComponent objectNeeds;

        public override bool Initalise(BaseObjectModel _baseObj)
        {
            this.objectNeeds = _baseObj.GetObjectComponent<ObjectNeedsComponent>();
            if (objectNeeds != null)
            {
                this.objectModel =_baseObj;
                this.SetText();
                return true;
            }
            return false;
        }

        public void Update()
        {
            this.hungerBar.UpdatePercentage(this.objectNeeds.GetFullness());
        }

        private void SetText()
        {

        }
    }
}