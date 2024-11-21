using UnityEngine;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private KeyCode previousKey = KeyCode.A; // Tecla para carro anterior
    [SerializeField] private KeyCode nextKey = KeyCode.D; // Tecla para próximo carro
    [SerializeField] private KeyCode previousArrowKey = KeyCode.LeftArrow; // Seta esquerda
    [SerializeField] private KeyCode nextArrowKey = KeyCode.RightArrow; // Seta direita
    private int currentCar;

    private void Awake()
    {
        SelectCar(0);

        // Atribuir ações aos botões
        previousButton.onClick.AddListener(() => ChangeCar(-1));
        nextButton.onClick.AddListener(() => ChangeCar(1));
    }

    private void Update()
    {
        // Detectar teclas pressionadas
        if (Input.GetKeyDown(previousKey) || Input.GetKeyDown(previousArrowKey)) // A ou Seta Esquerda
        {
            ChangeCar(-1);
        }
        else if (Input.GetKeyDown(nextKey) || Input.GetKeyDown(nextArrowKey)) // D ou Seta Direita
        {
            ChangeCar(1);
        }
    }

    private void ChangeCar(int _change)
    {
        currentCar += _change;
        SelectCar(currentCar);
    }

    private void SelectCar(int _index)
    {
        // Impedir que o índice ultrapasse os limites
        currentCar = Mathf.Clamp(_index, 0, transform.childCount - 1);

        // Controlar a interatividade dos botões
        previousButton.interactable = (currentCar != 0);
        nextButton.interactable = (currentCar != transform.childCount - 1);

        // Ativar apenas o carro atual e desativar os outros
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == currentCar);
        }
    }
}


