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
        private ObjectCompositionComponent objectComposition;

        public void Initalise(ObjectCompositionComponent _objectComposition)
        {
            this.objectComposition = _objectComposition;
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
                this.textBox.SetText(itemRow.Count > 0 ? boxString + itemRow.ConcatStrings("\n") : "No items stored");
            }
        }
    }
}