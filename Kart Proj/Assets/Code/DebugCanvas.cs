using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    [Header("Text Boxes")]
    [SerializeField]
    TextMeshProUGUI curSpeedTxt;
    [SerializeField]
    TextMeshProUGUI driftPowerTxt;
    [SerializeField]
    TextMeshProUGUI isDriftTxt;

    [Header("Car"), SerializeField]
    CarSystem car;

    // Update is called once per frame
    void Update()
    {
        curSpeedTxt.text = $"Current Speed: {car.currentSpeed.ToString("0.0")}";
        driftPowerTxt.text = $"Drift Power: {car.driftPower.ToString("0.0")} (lvl {car.GetDriftLevel()})";
        isDriftTxt.text = $"Is Drifting? {car.drifting}";
    }
}
