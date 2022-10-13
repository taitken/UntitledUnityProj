using System;
using System.Collections;
using System.Collections.Generic;
using ObjectComponents;
using TMPro;
using UnityEngine;
using UtilityClasses;

namespace UI.Panel
{
    public class StorageTab : BaseObjectTabContent
    {
        public TextMeshProUGUI textBox;
        public BaseObjectModel objectModel;
        private ObjectStorageComponent storageComponent;

        public override bool Initalise(BaseObjectModel _baseObj)
        {
            this.storageComponent = _baseObj.GetObjectComponent<ObjectStorageComponent>();
            if (storageComponent != null)
            {
                this.objectModel = _baseObj;
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
            if (this.storageComponent != null)
            {
                IList<string> itemRow = new List<string>();
                this.storageComponent.GetItems().ForEach(item =>
                {
                    itemRow.Add(LocalisationDict.GetMassString(item.mass) + " - " + item.itemType.ToString());
                });
                this.textBox.SetText(itemRow.Count > 0 ? itemRow.ConcatStrings("\n") : "No items stored");
            }
        }
    }
}