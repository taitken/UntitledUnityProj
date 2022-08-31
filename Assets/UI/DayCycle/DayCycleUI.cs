using System.Collections;
using System.Collections.Generic;
using GameControllers.Services;
using TMPro;
using UnityEngine;
using Zenject;

public class DayCycleUI : MonoBehaviour2
{
    private IDayCycleService dayCycleService;
        private TextMeshProUGUI textBox;

    [Inject]
    public void Construct(IDayCycleService _dayCycleService)
    {
        this.dayCycleService = _dayCycleService;
        this.textBox = this.GetComponentInChildren<TextMeshProUGUI>();
        this.dayCycleService.OnHourTickObservable.Subscribe(this, this.OnHourUpdate);
    }

    private void OnHourUpdate(int newHour)
    {
        this.textBox.SetText(newHour.ToString() + " : 00");
    }


}
