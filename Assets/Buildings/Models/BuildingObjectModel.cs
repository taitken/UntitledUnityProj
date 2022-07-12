using System;
using UnityEngine;

namespace Building.Models
{
    public class BuildingObjectModel : BaseModel
    {
        public BuildingObjectModel(Vector3Int _position) :base()
        {
            this.position = _position;
        }
        public Vector3Int position { get; set; }
    }
}

