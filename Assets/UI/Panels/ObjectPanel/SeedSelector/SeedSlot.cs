using System;
using System.Collections;
using System.Collections.Generic;
using Building.Models;
using Crops.Models;
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
        public Color highlightedColor;
        public GameObject background;
        public TextMeshProUGUI cropName;
        public ItemThumbnail cropThumbnail;
        public eCropType cropType;
        public EventEmitter<eCropType?> onCropTypeSelectEmitter = new EventEmitter<eCropType?>();
        // Start is called before the first frame update

        public void Initialise(string _itemName, Sprite _itemThumbnail, eCropType _cropType)
        {
            this.originalColour = this.background.GetComponent<Image>().color;
            this.cropType = _cropType;
            this.cropName.SetText(_itemName);
            this.cropThumbnail.SetImage(_itemThumbnail);
        }

        public void OnMouseClick()
        {
            this.onCropTypeSelectEmitter.Emit(this.cropType);
        }

        public void SetBackgroundColor(Color newColor)
        {
            this.originalColour = newColor;
            if(this.background.GetComponent<Image>().color != this.highlightedColor)
            {
                this.background.GetComponent<Image>().color = newColor;
            }
        }

        public override void OnMouseEnter()
        {
            MouseIconSingleton.SetCursorTexure(GameControllers.Models.eMouseAction.Pointer);
            this.background.GetComponent<Image>().color = this.highlightedColor;
        }
        public override void OnMouseExit()
        {
            MouseIconSingleton.SetCursorTexure(GameControllers.Models.eMouseAction.None);
            this.background.GetComponent<Image>().color = this.originalColour;
        }
    }
}