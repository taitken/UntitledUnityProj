using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI.Models;

namespace UI
{
    public class ProductionBuildingContextWindow : ContextWindow
    {
        private CWContent cwContext;
        private CWTitle cwTitle;
        public ProductionBuildingContextWindowModel contextWindowModel;
        [Inject]
        public void Construct(ContextWindowModel _contextWindowModel)
        {
            this.contextWindowModel = _contextWindowModel as ProductionBuildingContextWindowModel;
            this.cwTitle = this.GetComponentInChildren<CWTitle>();
            this.cwTitle.setText(this.contextWindowModel.title);
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