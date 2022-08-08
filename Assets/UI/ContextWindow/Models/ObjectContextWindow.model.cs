using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Models
{
    public class ObjectContextWindowModel : ContextWindowModel
    {
        public IList<string> context { get; set; }
        public ObjectContextWindowModel(long _objectID, string _title, IList<string> _context) : base(_objectID, _title)
        {
            this.context = _context;
            this.contextType = eContextTypes.Object;
        }
    }
}

