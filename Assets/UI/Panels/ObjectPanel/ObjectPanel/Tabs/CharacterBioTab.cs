using System;
using System.Collections;
using System.Collections.Generic;
using ObjectComponents;
using TMPro;
using UI.GenericComponents;
using Unit.Models;
using UnityEngine;
using UtilityClasses;

namespace UI.Panel
{
    public class CharacterBioTab : BaseObjectTabContent
    {
        public TextMeshProUGUI charName;
        public TextMeshProUGUI charSex;
        public TextMeshProUGUI charState;
        private UnitModel unitModel;
        public override bool Initalise(BaseObjectModel _baseObj)
        {
            if (_baseObj is UnitModel)
            {
                this.unitModel = _baseObj as UnitModel;
                this.SetText();
                return true;
            }
            return false;
        }

        public void Update()
        {
            this.SetText();
        }

        private void SetText()
        {
            this.charName.SetText(this.unitModel.unitName);
            this.charSex.SetText(this.unitModel.unitSex.ToString());
            this.charState.SetText(this.unitModel.unitState.ToString());
        }
    }
}