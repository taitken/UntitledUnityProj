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
    public class CharacterNeedsTab : BaseTabContent
    {
        public TextMeshProUGUI textBox;
        public PlainProgressBar hungerBar;
        public BaseObjectModel objectModel;
        private ObjectNeedsComponent objectNeeds;

        public void Initalise(ObjectNeedsComponent _objectNeeds)
        {
            this.objectNeeds = _objectNeeds;
            this.SetText();
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