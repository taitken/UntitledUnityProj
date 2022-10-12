using System;
using UnityEngine;
using System.Collections.Generic;
using Item.Models;
using GameControllers.Models;
using ObjectComponents;

namespace Unit.Models
{
    [ObjectComponentAttribute(typeof(ObjectNeedsComponent))]
    [ObjectComponentAttribute(typeof(ObjectHitPointsComponent))]
    public class UnitModel : BaseObjectModel
    {
        public UnitModel(float _movespeed, Vector3 _localPosition, Vector3Int _position) : base(_position, new List<ItemObjectMass> { new ItemObjectMass(eItemType.OrganicMass, 400) })
        {
            this.moveSpeed = _movespeed;
            this.localPosition = _localPosition;
            this.maxCarryWeight = 400;
            this.spriteOffset = 0.08f;
            this.GetObjectComponent<ObjectHitPointsComponent>().Initalise(100);
            this.unitState = eUnitState.Idle;
        }
        public eUnitState unitState { get; set; }
        public string currentHealth { get; set; }
        public IList<Vector3Int> currentPath { get; set; }
        public string maxHealth { get; set; }
        public int maxCarryWeight { get; set; }
        public float moveSpeed { get; set; }
        public Vector3 localPosition { get; set; }
        public UnitOrderModel currentOrder { get; set; }
        public ObjectNeedsComponent needsComponent { get { return this.GetObjectComponent<ObjectNeedsComponent>(); } }
        public ObjectHitPointsComponent hitPointsComponent { get { return this.GetObjectComponent<ObjectHitPointsComponent>(); } }
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

