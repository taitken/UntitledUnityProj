using System;
using UnityEngine;
using System.Collections.Generic;
using Item.Models;
using GameControllers.Models;

namespace Unit.Models
{
    public class UnitModel : BaseModel
    {
        public UnitModel(float _movespeed, Vector3 _position) : base()
        {
            this.moveSpeed = _movespeed;
            this.position = _position;
            this.maxCarryWeight = 400;
        }
        public string currentHealth { get; set; }
        public IList<Vector3Int> currentPath { get; set; }
        public string maxHealth { get; set; }
        public int maxCarryWeight {get;set;}
        public float moveSpeed { get; set; }
        public Vector3 position { get; set; }
        public UnitOrderModel currentOrder { get; set; }
        public ItemObjectModel carriedItem { get; set; }
    }
}

