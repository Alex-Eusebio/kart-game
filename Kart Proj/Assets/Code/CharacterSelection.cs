using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private Button previousButton; 
    [SerializeField] private Button nextButton; 
    [SerializeField] private KeyCode previousKey = KeyCode.A; // Tecla para personagem anterior (A ou seta esquerda)
    [SerializeField] private KeyCode nextKey = KeyCode.D; // Tecla para próxima personagem (D ou seta direita)
    [SerializeField] private KeyCode previousArrowKey = KeyCode.LeftArrow; // Seta esquerda
    [SerializeField] private KeyCode nextArrowKey = KeyCode.RightArrow; // Seta direita
    private int currentCharacter;

    private void Awake()
    {
        SelectCharacter(0); // Seleciona a primeira personagem no início

        // Atribuir ações aos botões
        previousButton.onClick.AddListener(() => ChangeCharacter(-1));
        nextButton.onClick.AddListener(() => ChangeCharacter(1));
    }

    private void Update()
    {
        // Detectar teclas pressionadas para navegar entre as personagens
        if (Input.GetKeyDown(previousKey) || Input.GetKeyDown(previousArrowKey)) // A ou Seta Esquerda
        {
            ChangeCharacter(-1);
        }
        else if (Input.GetKeyDown(nextKey) || Input.GetKeyDown(nextArrowKey)) // D ou Seta Direita
        {
            ChangeCharacter(1);
        }
    }

    public void ChangeCharacter(int _change)
    {
        // Alterar a seleção do personagem com base no _change
        currentCharacter += _change;

        // Garantir que o índice de personagens não ultrapasse os limites
        currentCharacter = Mathf.Clamp(currentCharacter, 0, transform.childCount - 1);

        SelectCharacter(currentCharacter); 
    }

    private void SelectCharacter(int _index)
    {
        // Atualizar a interatividade dos botões
        previousButton.interactable = (_index != 0); // Desativa o botão "Anterior" na primeira personagem
        nextButton.interactable = (_index != transform.childCount - 1); // Desativa o botão "Próximo" na última personagem

        // Ativar apenas o personagem atual e desativar os outros
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
    }
}

