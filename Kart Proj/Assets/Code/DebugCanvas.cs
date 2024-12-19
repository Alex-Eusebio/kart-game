using EasyRoads3Dv3;
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
    [SerializeField] private TextMeshProUGUI positionTxt;
    [SerializeField] private ParticleSystem speedLines;
    [SerializeField] private Animator laspLapAnim;

    [Header("Car")]
    [SerializeField] private CarSystem car; // Referência ao CarSystem

    private void Start()
    {
        DisableSpeedLines();
    }

    void FixedUpdate()
    {
        if (FindAnyObjectByType<Goal>())
        {
            if (positionTxt)
            {
                int temp = FindAnyObjectByType<Goal>().GetPosition(car.transform.parent.GetComponentInChildren<LapManager>());
                string txt = "";
                switch (temp)
                {
                    case 0:
                        txt = "1st";
                        break;
                    case 1:
                        txt = "2nd";
                        break;
                    case 2:
                        txt = "3rd";
                        break;
                    case 3:
                        txt = "4th";
                        break;
                    default:
                        txt = "PROBLEM";
                        break;
                }

                positionTxt.text = txt;
            }
            
            timerTxt.text = FindAnyObjectByType<Goal>().GetTimer();
        }

        // Atualiza os textos com as informações do carro
        if (car != null)
        {
            if (curSpeedTxt)
                curSpeedTxt.text = $"Current Speed: {(car.currentSpeed+car.bonusSpeed).ToString("0.0")}";

            if (driftPowerTxt)
                driftPowerTxt.text = $"Drift Power: {car.driftPower.ToString("0")} (lvl {car.GetDriftLevel()})";

            if (car.special && isSpecialTxt)
                isSpecialTxt.text = $"Is Special Ready? {car.special.IsAvailable()}";

            if (isDriftTxt)
                isDriftTxt.text = $"Is Drifting? {car.drifting}";

            if (lapTxt)
                lapTxt.text = $"Lap {car.transform.parent.GetComponentInChildren<LapManager>().curLaps}/{car.transform.parent.GetComponentInChildren<LapManager>().maxLaps}";
        }
    }

    public void LaspLap()
    {
        laspLapAnim.SetTrigger("1");
    }

    public void EnableSpeedLines()
    {
        if (speedLines != null)
        {
            speedLines.Play();
        }
    }

    public void DisableSpeedLines()
    {
        if (speedLines != null)
        {
            speedLines.Clear();
            speedLines.Stop();
        }
    }
}

