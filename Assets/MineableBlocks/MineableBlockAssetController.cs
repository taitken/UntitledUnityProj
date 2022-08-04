using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineableBlocks.Models;

namespace GameControllers
{
    public class MineableBlockAssetController : MonoBehaviour2
    {

        public Texture2D[] blockSpriteSheets;
        public Sprite[][] blockSprites;

        public void Start()
        {
            this.blockSprites = new Sprite[blockSpriteSheets.Length][];
            this.blockSpriteSheets.ForEach((sheet, index) =>{
                this.blockSprites[index] =  Resources.LoadAll<Sprite>(sheet.name);
            });
        }

        public Sprite[] GetBlockSpriteSet(eMineableBlockType blockType)
        {
            if (this.blockSprites.Length > (int)blockType)
            {
                return this.blockSprites[(int)blockType];
            }
            else
            {
                this.ThrowMissingItemError(blockType);
                return null;
            }
        }

        private void ThrowMissingItemError(eMineableBlockType blockType)
        {
                Debug.LogException(new System.Exception("Chosen block type sprite set has not been added to the Mineable Block Asset Controller. Attemped type: " + blockType.ToString()));
        }
    }
}