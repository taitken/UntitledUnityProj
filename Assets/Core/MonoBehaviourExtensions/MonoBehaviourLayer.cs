using System;
using System.Collections.Generic;
using Environment;
using GameControllers.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UtilityClasses;

namespace UnityEngine
{
    public abstract class MonoBehaviourLayer : MonoBehaviour2
    {
        public const int MAP_WIDTH = 100;
        public const int MAP_HEIGHT = 100;
        protected Tilemap tilemap;
        LayerCollider layerCollider;
        protected void InitiliseMonoLayer(LayerCollider.Factory _layerColliderFactory, Vector2 _size, string _layer)
        {
            this.tilemap = this.GetComponent<Tilemap>();
            IList<Action> callbacks = new List<Action>(){this.OnClickedByUser, this.OnMouseEnter, this.OnMouseExit};
            IList<Action<DragEventModel>> dragCallbacks = new List<Action<DragEventModel>>(){this.OnDrag, this.OnDragEnd};
            this.layerCollider = _layerColliderFactory.Create(_size, _layer, callbacks, dragCallbacks);
            this.layerCollider.gameObject.transform.SetParent(this.transform);
        }

        protected Vector3 GetLocalPositionOfCellAtMouse()
        {
            Vector3 cameraPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return this.tilemap.CellToLocal(this.tilemap.LocalToCell(new Vector3(cameraPoint.x + 0.05f, cameraPoint.y + 0.05f)));
        }

        protected Vector3Int GetCellCoorAtMouse()
        {
            Vector3 cameraPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return this.tilemap.LocalToCell(new Vector3(cameraPoint.x + 0.05f, cameraPoint.y + 0.05f));
        }
    }
}