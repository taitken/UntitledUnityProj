using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI.Models;
using TMPro;

namespace UI
{
    public class ContextWindow : MonoBehaviour2
    {
        public CWContent cwContext;
        public CWTitle cwTitle;
        public RectTransform rectTransform;
        public ContextWindowModel contextWindowModel;
        [Inject]
        public void Construct(ContextWindowModel _contextWindowModel)
        {
            this.contextWindowModel = _contextWindowModel;
            this.rectTransform = this.GetComponent<RectTransform>();
        }
        // Start is called before the first frame update
        void Start()
        {
            this.cwTitle.setText(this.contextWindowModel.title);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public class Factory : PlaceholderFactory<ContextWindowModel, ContextWindow>
        {
        }
    }
}