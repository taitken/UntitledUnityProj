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
    public class IconButtonSlot : MonoBehaviour2
    {
        public Color originalColour;
        public Color highlightedColor;
        public GameObject background;
        public TextMeshProUGUI title;
        public ItemThumbnail thumbnail;
        public int returnEnemurator;
        public EventEmitter<int> onButtonSelectEmitter = new EventEmitter<int>();
        // Start is called before the first frame update

        public IconButtonSlot CreateNew(Transform _parentTransform, string _title, Sprite _thumbnail, int _returnEnemurator)
        {
            IconButtonSlot newSlot = Instantiate(this, Vector3.zero, default(UnityEngine.Quaternion));
            newSlot.transform.parent = _parentTransform;
            newSlot.gameObject.SetActive(true);
            newSlot.Initialise(_title, _thumbnail, _returnEnemurator);
            return newSlot;
        }

        public void Initialise(string _title, Sprite _thumbnail, int _returnEnemurator)
        {
            this.originalColour = this.background.GetComponent<Image>().color;
            this.returnEnemurator = _returnEnemurator;
            this.title.SetText(_title);
            this.thumbnail.SetImage(_thumbnail);
        }

        public void OnMouseClick()
        {
            this.onButtonSelectEmitter.Emit(this.returnEnemurator);
        }

        public void SetBackgroundColor(Color newColor)
        {
            this.originalColour = newColor;
            if (this.background.GetComponent<Image>().color != this.highlightedColor)
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