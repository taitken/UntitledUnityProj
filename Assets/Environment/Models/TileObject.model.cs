using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Environment.Models
{
    public class TileObjectModel : BaseObjectModel
    {
        public TileObjectModel(Vector3Int _position, IList<ItemObjectMass> _item) :base(_position, _item)
        {
            
        }
    }
}

