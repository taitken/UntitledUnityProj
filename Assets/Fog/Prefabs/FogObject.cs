using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Environment.Models;
using Zenject;
using UtilityClasses;
using Item.Models;
using UI.Services;
using UI.Models;
using System;
using ObjectComponents;

namespace Environment
{
    public class FogObject : MonoBaseObject
    {
        private IEnvironmentService environmentService;
        public FogModel fogModel;

        [Inject]
        public void Construct(IEnvironmentService _environmentService,
                                FogModel _fogModel)
        {
            this.environmentService = _environmentService;
            this.fogModel = _fogModel;
        }

        public override void OnSelect()
        {
            
        }

        public override void OnMouseEnter()
        {

        }

        public override void OnMouseExit()
        {
            
        }

        public override void OnClickedByUser()
        {

        }

        public override BaseObjectModel GetBaseObjectModel()
        {
            return this.fogModel;
        }

        protected override void BeforeDeath()
        {
            this.environmentService.RemoveFogObject(this.fogModel.position);
            base.BeforeDeath();
        }

        public void UpdateSprite(int spriteID)
        {
            
        }

        public class Factory : PlaceholderFactory<FogModel, FogObject>
        {
        }
    }
}