using System;
using UnityEngine;

namespace Environment.Models
{
    public class MineableObjectModel : BaseModel
    {
        public MineableObjectModel(Vector3 _localPosition) :base()
        {
            this.localPosition = _localPosition;
        }
        public Vector3 localPosition { get; set; }
    }
}

