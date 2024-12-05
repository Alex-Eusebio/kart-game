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

    [Header("Car")]
    [SerializeField] private CarSystem car; // Referência ao CarSystem
    [SerializeField] private Transform followTarget; // Transform do objeto a ser seguido

    private Vector3 offset; // Offset para posicionar o canvas em relação ao carro

    void Start()
    {
        followTarget = car.transform;
        // Defina o offset inicial baseado na posição relativa atual
        if (followTarget != null)
        {
            offset = transform.position - followTarget.position;
        }
    }

    void Update()
    {
        // Atualiza a posição do DebugCanvas para acompanhar o carro
        if (followTarget != null)
        {
            transform.position = followTarget.position + offset;
        }

        // Atualiza os textos com as informações do carro
        if (car != null)
        {
            try
            {
                curSpeedTxt.text = $"Current Speed: {(car.currentSpeed+car.bonusSpeed).ToString("0.0")}";
                driftPowerTxt.text = $"Drift Power: {car.driftPower.ToString("0")} (lvl {car.GetDriftLevel()})";
                isSpecialTxt.text = $"Is Special Ready? {car.special.IsAvailable()}";
                isDriftTxt.text = $"Is Drifting? {car.drifting}";
                timerTxt.text = FindAnyObjectByType<Goal>().GetTimer();
            }
            catch
            {
                Debug.LogWarning("Erro ao atualizar os textos do DebugCanvas.");
            }
        }
    }
}

