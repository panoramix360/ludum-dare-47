using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventIcon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI eventName;
    [SerializeField] private TextMeshProUGUI waterTxt;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI energyTxt;

    public void SetEventIconText(string eventName, string waterText, string hpText, string energyText)
    {
        this.eventName.text = eventName;
        this.waterTxt.text = waterText;
        this.hpTxt.text = hpText;
        this.energyTxt.text = energyText;
    }
}
