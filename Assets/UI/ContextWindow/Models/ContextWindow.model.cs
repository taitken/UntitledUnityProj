using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Models
{
    public class ContextWindowModel : BaseModel
    {
        public long objectID {get;set;}
        public string title { get; set; }
        public ContextWindowModel(long _objectID, string _title) : base()
        {
            this.objectID = _objectID;
            this.title = _title;
        }
    }
}

