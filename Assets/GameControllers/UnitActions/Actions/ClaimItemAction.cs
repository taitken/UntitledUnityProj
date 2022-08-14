
using System;
using System.Collections.Generic;
using Building.Models;
using GameControllers.Services;
using Item.Models;
using Unit.Models;
using UnityEngine;
using UtilityClasses;

namespace UnitAction
{
    public class ClaimItemAction : IUnitAction
    {
        private UnitModel unit;
        private ItemObjectModel itemObjModel;
        private Subscription subscription;
        private IItemObjectService itemObjectService;
        private decimal requestedMass;
        public bool completed { get; set; } = false;
        public bool cancel { get; set; } = false;
        public ClaimItemAction(ItemObjectModel _itemObjModel,
                                IItemObjectService _itemObjectService,
                                decimal _requestedMass)
        {
            this.itemObjModel = _itemObjModel;
            this.cancel = this.itemObjModel == null;
            this.requestedMass = _requestedMass;
            this.itemObjectService = _itemObjectService;
            this.subscription = this.itemObjectService.itemObseravable.SubscribeQuietly(null, items =>
            {
                if (!items.Any(item => { return item.ID == this.itemObjModel.ID; })) this.CancelAction();
            });
        }
        public bool CheckCompleted()
        {
            if (this.completed)
            {
                this.unclaimMass();
                this.subscription.unsubscribe();
                return true;
            }
            return false;
        }
        public void CancelAction()
        {
            this.unclaimMass();
            this.subscription.unsubscribe();
            this.cancel = true;
        }
        private void unclaimMass()
        {
            this.itemObjModel.claimedMass -= this.requestedMass;
        }
        public bool PerformAction()
        {
            if (this.itemObjModel == null)
            {
                this.cancel = true;
                return false;
            }
            this.itemObjModel.claimedMass += this.requestedMass;
            this.completed = true;
            return true;
        }
    }
}