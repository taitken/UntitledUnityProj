
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GenericComponents
{
    public class PlainProgressBar : MonoBehaviour
    {
        public ProgressBlit progressBlit;
        private RectTransform rectTransform;
        private float barWidth;
        private int visibleObjects;
        // Start is called before the first frame update
        void Start()
        {
            this.rectTransform = this.progressBlit.GetComponent<RectTransform>();
            this.barWidth = this.rectTransform.rect.width;
            this.UpdatePercentage(0);
        }

        public void UpdatePercentage(float percentage)
        {
            this.rectTransform.sizeDelta = new Vector2(this.barWidth * percentage, this.rectTransform.sizeDelta.y);
            this.rectTransform.anchoredPosition = new Vector3(this.rectTransform.sizeDelta.x + 1, this.rectTransform.anchoredPosition.y);
        }
    }
}
