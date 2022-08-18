using System;
using System.Collections.Generic;
using Building.Models;
using Environment;
using Environment.Models;
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
            IList<Action> callbacks = new List<Action>() { this.OnClickedByUser, this.OnMouseEnter, this.OnMouseExit };
            IList<Action<DragEventModel>> dragCallbacks = new List<Action<DragEventModel>>() { this.OnDrag, this.OnDragEnd };
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


        protected void MoveObjectOffInvalidPosition(IList<MonoBaseObject> objectsToMove, Vector3Int positionToMoveOff, BuildingObjectModel[,] _walls, MineableObjectModel[,] _mineableBlocks)
        {
            IList<MonoBaseObject> objects = objectsToMove.Filter(itemObj => { return itemObj.GetBaseObjectModel().position == positionToMoveOff; });
            objects.ForEach(objectToMove =>
            {
                bool itemMoved = false;
                if (!itemMoved) { itemMoved = this.CheckAndMoveItem(positionToMoveOff.x - 1, positionToMoveOff.y, objectToMove, _walls, _mineableBlocks); }
                if (!itemMoved) { itemMoved = this.CheckAndMoveItem(positionToMoveOff.x + 1, positionToMoveOff.y, objectToMove, _walls, _mineableBlocks); }
                if (!itemMoved) { itemMoved = this.CheckAndMoveItem(positionToMoveOff.x, positionToMoveOff.y - 1, objectToMove, _walls, _mineableBlocks); }
                if (!itemMoved) { itemMoved = this.CheckAndMoveItem(positionToMoveOff.x, positionToMoveOff.y + 1, objectToMove, _walls, _mineableBlocks); }
                if (!itemMoved) { itemMoved = this.CheckAndMoveItem(positionToMoveOff.x - 1, positionToMoveOff.y - 1, objectToMove, _walls, _mineableBlocks); }
                if (!itemMoved) { itemMoved = this.CheckAndMoveItem(positionToMoveOff.x + 1, positionToMoveOff.y + 1, objectToMove, _walls, _mineableBlocks); }
                if (!itemMoved) { itemMoved = this.CheckAndMoveItem(positionToMoveOff.x + 1, positionToMoveOff.y - 1, objectToMove, _walls, _mineableBlocks); }
                if (!itemMoved) { itemMoved = this.CheckAndMoveItem(positionToMoveOff.x - 1, positionToMoveOff.y + 1, objectToMove, _walls, _mineableBlocks); }
            });
        }

        private bool CheckAndMoveItem(int x, int y, MonoBaseObject baseObject, BuildingObjectModel[,] _walls, MineableObjectModel[,] _mineableBlocks)
        {
            if (this.CheckIfSpotFree(x, y, _walls, _mineableBlocks))
            {
                this.SetItemPosition(baseObject, new Vector3Int(x, y));
                return true;
            }
            return false;
        }

        private bool CheckIfSpotFree(int x, int y, BuildingObjectModel[,] _walls, MineableObjectModel[,] _mineableBlocks)
        {
            return _walls[x, y] == null && _mineableBlocks[x, y] == null;
        }

        private void SetItemPosition(MonoBaseObject baseObject, Vector3Int pos)
        {
            baseObject.transform.position = this.tilemap.CellToLocal(pos);
            baseObject.GetBaseObjectModel().position = pos;
        }
    }
}