using System;
using UnityEngine;
using System.Collections.Generic;
using Item.Models;
using GameControllers.Models;
using ObjectComponents;

namespace Unit.Models
{
    public class UnitModel : BaseObjectModel
    {
        public UnitModel(float _movespeed, Vector3 _localPosition, Vector3Int _position) : base(_position, new List<ItemObjectMass> { new ItemObjectMass(eItemType.OrganicMass, 400) })
        {
            this.moveSpeed = _movespeed;
            this.localPosition = _localPosition;
            this.maxCarryWeight = 400;
            this.spriteOffset = 0.08f;
            this.needsComponent = new ObjectNeedsComponent();
            this.hitPointsComponent = new ObjectHitPointsComponent(100);
            this.objectComponents.Add(this.needsComponent);
            this.objectComponents.Add(this.hitPointsComponent);
        }
        public string currentHealth { get; set; }
        public IList<Vector3Int> currentPath { get; set; }
        public string maxHealth { get; set; }
        public int maxCarryWeight { get; set; }
        public float moveSpeed { get; set; }
        public Vector3 localPosition { get; set; }
        public UnitOrderModel currentOrder { get; set; }
        public ObjectNeedsComponent needsComponent { get; set; }
        public ObjectHitPointsComponent hitPointsComponent { get; set; }
        public ItemObjectModel carriedItem
        {
            get
            {
                return this._carriedItem;
            }
            set
            {
                this._carriedItem = value;
                if (this._carriedItem != null) this._carriedItem.itemState = ItemObjectModel.eItemState.OnCharacter;
            }
        }
        private ItemObjectModel _carriedItem;
    }
}

