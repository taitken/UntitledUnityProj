using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Models
{
    public class ObjectPanelModel : BasePanelModel
    {
        public ObjectPanelModel(long _objectID, string _title) : base(_objectID, _title, ePanelTypes.ObjectInfo)
        {
            this.objectID = _objectID;
            this.title = _title;
        }
    }
}

