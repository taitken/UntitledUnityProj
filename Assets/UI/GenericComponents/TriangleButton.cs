using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Zenject;

namespace UI.GenericComponents
{
    public class TriangleButton : MonoBehaviour2
    {
        public Texture2D hoverCursorTexure;
        public EventEmitter onClickEmitter = new EventEmitter();

        public override void OnMouseEnter()
        {
            Cursor.SetCursor(this.hoverCursorTexure, new Vector2(12, 0), CursorMode.Auto);
        }
        public override void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        public void OnMouseClick()
        {
            this.onClickEmitter.Emit();
        }
    }
}
