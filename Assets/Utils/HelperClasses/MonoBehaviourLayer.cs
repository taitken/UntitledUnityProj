using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UtilityClasses;

namespace UnityEngine
{
    public abstract class MonoBehaviourLayer : MonoBehaviour2
    {
        public Tilemap tilemap;

        protected void InitiliseMonoLayer()
        {
            this.tilemap = this.GetComponent<Tilemap>();

        }

        protected Vector3 GetCellCoorAtMouse()
        {
            Vector3 cameraPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return this.tilemap.CellToLocal(this.tilemap.LocalToCell(new Vector3(cameraPoint.x + 0.05f, cameraPoint.y + 0.05f)));
        }
    }
}