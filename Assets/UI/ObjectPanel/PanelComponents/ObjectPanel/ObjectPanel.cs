using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Models;
using UnityEngine;

namespace UI.Panel
{
    public class ObjectPanel : BasePanel
    {
        // Start is called before the first frame update
        public override void Construct(BasePanelModel panelWindowModel)
        {
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts.ForEach(text =>
            {
                if (text.tag == "UiHeader") text.SetText(panelWindowModel.title);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
