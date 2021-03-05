using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventIcon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI eventName;
    [SerializeField] private TextMeshProUGUI waterTxt;
    [SerializeField] private TextMeshProUGUI structureTxt;
    [SerializeField] private TextMeshProUGUI energyTxt;

    public void SetEventIconText(string eventName, string waterText, string structureText, string energyText)
    {
        this.eventName.text = eventName;
        this.waterTxt.text = waterText;
        this.structureTxt.text = structureText;
        this.energyTxt.text = energyText;
    }
}
