
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GenericComponents
{
    public class ProgressBar : MonoBehaviour
    {
        private const float DIVIDER = 6.66f;
        public ProgressBlit[] progressBlits;
        private int visibleObjects;
        // Start is called before the first frame update
        void Start()
        {
            this.progressBlits = this.GetComponentsInChildren<ProgressBlit>();
            for (int i = 0; i < progressBlits.Length; i++)
            {
                progressBlits[i].GetComponent<Image>().enabled = false;
            }
            this.UpdatePercentage(0);
        }

        public void UpdatePercentage(float percentage)
        {
            int newVisibleObjects = ((int)(percentage / DIVIDER));
            if (newVisibleObjects != this.visibleObjects)
            {
                for (int i = 0; i < progressBlits.Length; i++)
                {
                    progressBlits[i].GetComponent<Image>().enabled = i < newVisibleObjects;
                }
            }
            this.visibleObjects = newVisibleObjects;
        }
    }
}
