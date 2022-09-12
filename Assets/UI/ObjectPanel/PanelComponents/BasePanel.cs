using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Models;
using UnityEngine;

public abstract class BasePanel : MonoBehaviour2
{
    private IList<IBaseService> services;
    // Start is called before the first frame update
    public abstract void Construct(BasePanelModel panelWindowModel);

    public void InjectServices(IList<IBaseService> _services)
    {
        this.services = _services;
    }

    protected T GetService<T>() where T : IBaseService
    {
        return this.GetService<T>(this.services);
    }
}
