using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Building.Models;
using GameControllers.Services;
using GameControllers.Models;
using UI.Services;
using UI.Models;

namespace UI
{
    public class BuildCategoryButton : HiveBaseButton
    {
        public eBuildingCategory buildingCategory;
        private IUiPanelService uiService;
        public Button buttonComponent;

        [Inject]
        public void Construct(IUiPanelService _panelService)
        {
            this.uiService = _panelService;
        }
        // Start is called before the first frame update
        void Start()
        {
            this.buttonComponent = GetComponent<Button>();
            this.buttonComponent.onClick.AddListener(this.SelectBuildingCategory);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SelectBuildingCategory()
        {
            this.uiService.selectedBuildingPanels.Set(new List<BasePanelModel>{new BuildingSelectorPanelModel(this.buildingCategory.ToString(), this.buildingCategory)});
        }
    }
}