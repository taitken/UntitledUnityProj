using System.Collections;
using System.Collections.Generic;
using ObjectComponents;
using TMPro;
using UnityEngine;
using UtilityClasses;

namespace UI.Panel
{
    public class StorageTab : MonoBehaviour2
    {
        public TextMeshProUGUI textBox;
        private ObjectStorage storageComponent;

        public void Initalise(ObjectStorage _storageComponent)
        {
            this.storageComponent = _storageComponent;
            this.SetText();
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