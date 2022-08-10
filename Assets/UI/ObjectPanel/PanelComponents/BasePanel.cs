using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Models;
using UnityEngine;

public abstract class BasePanel : MonoBehaviour2
{
    // Start is called before the first frame update
    public abstract void Construct(BasePanelModel panelWindowModel);
}
