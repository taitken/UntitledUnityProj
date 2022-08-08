using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ItemSlot : MonoBehaviour2
    {
        public ItemThumbnail itemThumbnail;
        public TextMeshProUGUI requiredNumber;
        public TextMeshProUGUI currentNumber;
        // Start is called before the first frame update
        public void SetItemSprite(Sprite sprite)
        {
            this.itemThumbnail.SetImage(sprite);
        }
        public void SetRequiredNumber(string text)
        {
            this.requiredNumber.SetText(text);
        }

        public void SetCurrentNumber(string text)
        {
            this.currentNumber.SetText(text);
        }
    }
}
