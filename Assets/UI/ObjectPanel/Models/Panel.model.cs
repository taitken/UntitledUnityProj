using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Models
{
    public class ObjectPanelModel : BasePanelModel
    {
        public BaseObjectModel objectModel;
        public ObjectPanelModel(long _objectID, string _title, BaseObjectModel _objectModel) : base(_objectID, _title, ePanelTypes.ObjectInfo)
        {
            this.objectID = _objectID;
            this.title = _title;
            this.objectModel = _objectModel;
        }
    }
}

