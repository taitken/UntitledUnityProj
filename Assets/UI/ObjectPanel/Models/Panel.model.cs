using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Models
{
    public class PanelModel : BaseModel
    {
        public long objectID { get; set; }
        public string title { get; set; }
        public ePanelTypes panelType { get; set; }
        public PanelModel(long _objectID, string _title, ePanelTypes _panelType) : base()
        {
            this.objectID = _objectID;
            this.title = _title;
            this.panelType = _panelType;
        }
    }
}

