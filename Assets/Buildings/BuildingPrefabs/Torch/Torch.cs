using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Building
{
    public class Torch : DecorBuildingObject
    {
        public Light2D lighting2D;
        private Vector3 flickerDefaultScale;
        protected override void OnCreation()
        {
            base.OnCreation();
            this.flickerDefaultScale = this.lighting2D.transform.localScale;
        }

        public void Flicker(int frameIndex)
        {
            float scale = 1;
            switch (frameIndex)
            {
                case 1:
                    scale = 1;
                    break;
                case 2:
                    scale = 1.01f;
                    break;
                case 3:
                    scale = 1.005f;
                    break;
            }
            this.lighting2D.transform.localScale = new Vector3(this.flickerDefaultScale.x * scale, this.flickerDefaultScale.y * scale);
        }
    }
}