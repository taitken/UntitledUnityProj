using System;
using System.Collections.Generic;
using Building.Models;
using UnityEngine;

namespace GameControllers.Models
{
    public class DragEventModel
    {

        public Vector3 initialDragLocation { get; }
        public Vector3 currentDragLocation { get; set; }
        public IList<GameObject> draggedObjects {get;}
        public DragEventModel(Vector3 initialLocation, IList<GameObject> _draggedObjects)
        {
            this.initialDragLocation = initialLocation;
            this.draggedObjects = _draggedObjects;
            this.currentDragLocation = initialLocation;
        }
    }
}

