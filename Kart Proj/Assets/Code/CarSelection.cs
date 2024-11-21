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
    private int currentCar = 0;

    private void Awake()
    {
        // Inicia a seleção com o primeiro carro
        SelectCar(currentCar);

        // Atribui ações aos botões
        previousButton.onClick.AddListener(() => ChangeCar(-1));
        nextButton.onClick.AddListener(() => ChangeCar(1));
    }

    private void Update()
    {
        // Detectar teclas pressionadas
        if (Input.GetKeyDown(previousKey) || Input.GetKeyDown(previousArrowKey)) // A ou seta esquerda
        {
            ChangeCar(-1);
        }
        else if (Input.GetKeyDown(nextKey) || Input.GetKeyDown(nextArrowKey)) // D ou seta direita
        {
            ChangeCar(1);
        }
    }

    private void ChangeCar(int _change)
    {
        // Alterar o índice do carro
        currentCar += _change;

        // Garantir que o índice do carro não ultrapasse os limites (primeiro e último)
        if (currentCar < 0)
        {
            currentCar = 0; // Não pode ir antes do primeiro carro
        }
        else if (currentCar >= transform.childCount)
        {
            currentCar = transform.childCount - 1; // Não pode ultrapassar o último carro
        }

        // Atualizar a seleção de carros
        SelectCar(currentCar);
    }

    private void SelectCar(int _index)
    {
        // Atualizar o índice do carro
        currentCar = _index;

        // Ativar e desativar os carros
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == currentCar);
        }

        // Atualizar os botões
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        // Os botões de "anterior" e "próximo" são desabilitados quando o carro atual é o primeiro ou o último
        previousButton.interactable = (currentCar > 0);
        nextButton.interactable = (currentCar < transform.childCount - 1);
    }
}



