using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    [Header("Text Boxes")]
    [SerializeField] private TextMeshProUGUI curSpeedTxt;
    [SerializeField] private TextMeshProUGUI driftPowerTxt;
    [SerializeField] private TextMeshProUGUI isDriftTxt;
    [SerializeField] private TextMeshProUGUI isSpecialTxt;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private TextMeshProUGUI lapTxt;

    [Header("Car")]
    [SerializeField] private CarSystem car; // Referência ao CarSystem

    void Update()
    {
        if (FindAnyObjectByType<Goal>())
            timerTxt.text = FindAnyObjectByType<Goal>().GetTimer();

        // Atualiza os textos com as informações do carro
        if (car != null)
        {
            curSpeedTxt.text = $"Current Speed: {(car.currentSpeed+car.bonusSpeed).ToString("0.0")}";
            driftPowerTxt.text = $"Drift Power: {car.driftPower.ToString("0")} (lvl {car.GetDriftLevel()})";
            isSpecialTxt.text = $"Is Special Ready? {car.special.IsAvailable()}";
            isDriftTxt.text = $"Is Drifting? {car.drifting}";
            lapTxt.text = $"Lap {car.transform.parent.GetComponentInChildren<LapManager>().curLaps}/{car.transform.parent.GetComponentInChildren<LapManager>().maxLaps}";
        }
    }
}

