using System;
using System.Collections;
using System.Collections.Generic;
using ObjectComponents;
using TMPro;
using UnityEngine;
using UtilityClasses;

namespace UI.Panel
{
    public class DetailsTab : BaseTabContent
    {
        public TextMeshProUGUI textBox;
        public BaseObjectModel objectModel;
        private ObjectCompositionComponent objectComposition;

        public void Initalise(ObjectCompositionComponent _objectComposition, BaseObjectModel _objectModel)
        {
            this.objectComposition = _objectComposition;
            this.objectModel = _objectModel;
            this.SetText();
        }

        public void Update()
        {
            this.SetText();
        }

        private void SetText()
        {
            if (this.objectComposition != null)
            {
                string boxString = "This object is made up of the following items: \n";
                IList<string> itemRow = new List<string>();
                this.objectComposition.GetComposition().ForEach(item =>
                {
                    itemRow.Add(LocalisationDict.GetMassString(item.mass) + " - " + item.itemType.ToString());
                });
                string objectCompositionString = itemRow.Count > 0 ? boxString + itemRow.ConcatStrings("\n") : "No items stored";
                this.textBox.SetText((this.objectModel.objectDescription != null ? this.objectModel.objectDescription + "\n" : "")
                                        + objectCompositionString);
            }
        }
    }
}