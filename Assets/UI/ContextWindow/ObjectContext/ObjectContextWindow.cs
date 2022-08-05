using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI.Models;
using TMPro;

namespace UI
{
    public class ObjectContextWindow : ContextWindow
    {
        private CWContent cwContext;
        private CWTitle cwTitle;
        public ContextWindowModel contextWindowModel;
        [Inject]
        public void Construct(ContextWindowModel _contextWindowModel)
        {
            this.contextWindowModel = _contextWindowModel;
            this.cwTitle = this.GetComponentInChildren<CWTitle>();
            this.cwTitle.setText(this.contextWindowModel.title);
            this.cwContext = this.GetComponentInChildren<CWContent>();
            this.cwContext.setText(this.contextWindowModel.context);
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public class Factory : PlaceholderFactory<ContextWindowModel, ObjectContextWindow>
        {
        }
    }
}