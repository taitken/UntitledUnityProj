using System;

namespace GameControllers.Models
{
    public class UnitActionModel : BaseModel
    {
        public eActionCategories actionCategory { get; set; }
        public string actionName { get; set; }
        public float priority { get; set; }
    }
}

