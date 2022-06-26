using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace Environment
{
    public class MineableHunk : MonoBehaviour2
    {
        public Sprite[] spriteList;
        private SpriteRenderer spriteRenderer;
        // Start is called before the first frame update
        void Start()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
            print("clicked");
        }

        public override void OnClickedByUser()
        {
            this.DestroySelf();
        }

        public void updateSprite(int spriteID)
        {
            this.spriteRenderer.sprite = this.spriteList[spriteID];
        }

    }
}