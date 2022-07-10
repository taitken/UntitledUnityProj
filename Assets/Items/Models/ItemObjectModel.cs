using System;
using UnityEngine;

namespace Item.Models
{
    public class ItemObjectModel : BaseModel
    {
        public ItemObjectModel(Vector3Int _position) :base()
        {
            this.position = _position;
        }
        public Vector3Int position { get; set; }
    }
}

