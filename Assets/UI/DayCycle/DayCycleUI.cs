using System.Collections;
using System.Collections.Generic;
using GameControllers.Services;
using TMPro;
using UI.GenericComponents;
using UnityEngine;
using UtilityClasses;
using Zenject;

public class DayCycleUI : MonoBehaviour2
{
    private IDayCycleService dayCycleService;
    private TextMeshProUGUI textBox;
    public TriangleButton speedDown;
    public TriangleButton speedUp;
    public TextMeshProUGUI speedText;


    [Inject]
    public void Construct(IDayCycleService _dayCycleService)
    {
        this.dayCycleService = _dayCycleService;
        this.textBox = this.GetComponentInChildren<TextMeshProUGUI>();
        this.dayCycleService.OnHourTickObservable.Subscribe(this, this.OnHourUpdate);
        this.speedDown.onClickEmitter.OnEmit(() => { GameTime.DecreaseGameSpeed(1); this.UpdateGameSpeedText(); });
        this.speedUp.onClickEmitter.OnEmit(() => { GameTime.IncreaseGameSpeed(1); this.UpdateGameSpeedText(); });
        this.UpdateGameSpeedText();
    }

    private void OnHourUpdate(int newHour)
    {
        this.textBox.SetText(newHour.ToString() + " : 00");
    }

    private void UpdateGameSpeedText()
    {
        this.speedText.SetText("x" + GameTime.gameSpeedMultiplier.ToString());
    }


}
