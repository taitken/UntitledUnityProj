using System;
using System.Collections;
using System.Collections.Generic;
using Building.Models;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;
using UtilityClasses;

namespace UI.Panel
{
    public class SeedSlot : MonoBehaviour2
    {
        public Color originalColour;
        public GameObject background;
        public TextMeshProUGUI cropName;
        public ItemThumbnail cropThumbnail;
        private bool selected;
        // Start is called before the first frame update

        public void Initialise(string _itemName, Sprite _itemThumbnail)
        {
            this.selected = false;
            this.cropName.SetText(_itemName);
            this.cropThumbnail.SetImage(_itemThumbnail);
        }

        public void OnMouseClick()
        {

        }

        public override void OnMouseEnter()
        {
            MouseIconSingleton.SetCursorTexure(GameControllers.Models.eMouseAction.Pointer);
            this.originalColour = this.background.GetComponent<Image>().color;
            this.background.GetComponent<Image>().color = this.originalColour;
            this.background.GetComponent<Image>().color = new Color(0.96f, 1, 0.985f);
        }
        public override void OnMouseExit()
        {
            MouseIconSingleton.SetCursorTexure(GameControllers.Models.eMouseAction.None);
            this.background.GetComponent<Image>().color = this.originalColour;
        }


    }
}