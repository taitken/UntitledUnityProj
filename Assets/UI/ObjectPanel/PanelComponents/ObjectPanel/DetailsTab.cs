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
    public class DetailsTab : BaseObjectTabContent
    {
        public TextMeshProUGUI textBox;
        public BaseObjectModel objectModel;
        public PlainProgressBar hitPointsBar;
        private ObjectCompositionComponent objectComposition;
        private ObjectHitPointsComponent objectHitPoints;
        public override bool Initalise(BaseObjectModel _baseObj)
        {
            this.objectComposition = _baseObj.GetObjectComponent<ObjectCompositionComponent>();
            if (objectComposition != null)
            {
                this.objectHitPoints = _baseObj.GetObjectComponent<ObjectHitPointsComponent>();
                this.objectModel = _baseObj;
                this.SetText();
                return true;
            }
            return false;
        }

        public void Update()
        {
            this.SetText();
            this.UpdateHitPointsBar();
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

        private void UpdateHitPointsBar()
        {
            if (this.objectHitPoints != null)
            {
                this.hitPointsBar.UpdatePercentage(this.objectHitPoints.currentHitPoints / this.objectHitPoints.maxHitPoints);
            }
        }
    }
}