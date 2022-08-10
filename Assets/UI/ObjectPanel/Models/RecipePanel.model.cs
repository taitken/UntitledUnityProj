using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Models
{
    public class RecipePanelModel : BasePanelModel
    {

        public RecipePanelModel(long _objectID, string _title) : base(_objectID, _title, ePanelTypes.RecipeSelector)
        {

        }
    }
}

