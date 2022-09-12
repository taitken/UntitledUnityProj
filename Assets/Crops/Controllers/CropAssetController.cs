using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Building;
using Building.Models;
using Zenject;
using Environment.Models;
using UtilityClasses;
using Crops;
using Crops.Models;

namespace GameControllers
{
    public class CropAssetController : MonoBehaviour2
    {
        public Texture2D[] cropSpritesSheets;
        public Sprite[][] cropSprites;
        public void Initialise()
        {
            this.cropSprites = new Sprite[cropSpritesSheets.Length][];
            this.cropSpritesSheets.ForEach((sheet, index) =>
            {
                this.cropSprites[index] = Resources.LoadAll<Sprite>(sheet.name);
            });
        }

        public Sprite[] GetCropSpriteSet(eCropType cropType)
        {
            if (this.cropSprites.Length > (int)cropType)
            {
                return this.cropSprites[(int)cropType];
            }
            else
            {
                this.ThrowMissingSpriteError(cropType);
                return null;
            }
        }

        private void ThrowMissingSpriteError(eCropType cropType)
        {
            Debug.LogException(new System.Exception("Chosen crop type sprite set has not been added to the Crop Asset Controller. Attemped type: " + cropType.ToString()));
        }
    }
}