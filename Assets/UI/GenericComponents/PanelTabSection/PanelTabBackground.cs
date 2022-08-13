using System.Collections;
using System.Collections.Generic;
using GameControllers.Models;
using UnityEngine;
using UnityEngine.UI;
using UtilityClasses;

public class PanelTabBackground : MonoBehaviour2
{
    public EventEmitter<PanelTabBackground> onClickEmitter = new EventEmitter<PanelTabBackground>();
    private Color originalColour;
    public override void OnMouseEnter()
    {
        MouseIconService.SetCursorTexure(eMouseAction.Pointer);
        this.originalColour = this.GetComponent<Image>().color;
        this.GetComponent<Image>().color = GameColors.Lighten(GetComponent<Image>().color, 1.55f);
    }
    public override void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        this.GetComponent<Image>().color = this.originalColour;
    }

    public void OnMouseClick()
    {
        this.onClickEmitter.Emit(this);
    }
}
