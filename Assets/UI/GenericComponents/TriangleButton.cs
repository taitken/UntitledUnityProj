using System.Collections;
using System.Collections.Generic;
using GameControllers.Models;
using UnityEngine;
using UnityEngine.UI;
using UtilityClasses;
using Zenject;

namespace UI.GenericComponents
{
    public class TriangleButton : MonoBehaviour2
    {
        public Image triangleImage;
        public EventEmitter onClickEmitter = new EventEmitter();
        public Color originalColour;

        public override void OnMouseEnter()
        {
            MouseIconSingleton.SetCursorTexure(eMouseAction.Pointer);
            this.originalColour = triangleImage.color;
            this.triangleImage.color = GameColors.Lighten(triangleImage.color, 1.15f);
        }
        public override void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            this.triangleImage.color = this.originalColour;
        }

        public void OnMouseClick()
        {
            this.onClickEmitter.Emit();
        }
    }
}
