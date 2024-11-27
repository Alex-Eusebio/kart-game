using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Button[] menuButtons; // Array de botões no menu
    private int currentIndex = 0; // Índice do botão atualmente selecionado
    public string playSceneName; // Nome da cena para o botão Play
    public GameObject settingsPanel; // Painel de configurações

    void Start()
    {
        if (menuButtons.Length > 0)
        {
            HighlightButton(currentIndex); // Inicia destacando o primeiro botão
        }
    }

    void Update()
    {
        HandleInput();
    }

    // Gerencia a navegação do menu
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(-1); // Move para cima
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(1); // Move para baixo
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmSelection(); // Confirma a seleção ao pressionar Enter ou Espaço
        }
    }

    // Move a seleção no menu
    private void MoveSelection(int direction)
    {
        // Remove destaque do botão atual
        RemoveHighlight(currentIndex);

        // Calcula o novo índice
        currentIndex += direction;

        // Certifica-se de que o índice está dentro dos limites
        if (currentIndex < 0)
        {
            currentIndex = menuButtons.Length - 1; // Vai para o último botão
        }
        else if (currentIndex >= menuButtons.Length)
        {
            currentIndex = 0; // Volta para o primeiro botão
        }

        // Destaca o novo botão
        HighlightButton(currentIndex);
    }

    // Destaca o botão selecionado
    private void HighlightButton(int index)
    {
        if (menuButtons[index] != null)
        {
            menuButtons[index].Select(); // Destaca o botão
        }
    }

    // Remove o destaque do botão
    private void RemoveHighlight(int index)
    {
        if (menuButtons[index] != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // Remove o foco
        }
    }

    // Confirma a ação do botão selecionado
    private void ConfirmSelection()
    {
        if (menuButtons[currentIndex] != null)
        {
            menuButtons[currentIndex].onClick.Invoke(); // Invoca o evento do botão
        }
    }

    // Botão Play
    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(playSceneName))
        {
            SceneManager.LoadScene(playSceneName); // Carrega a cena do jogo
        }
        else
        {
            Debug.LogError("Play scene name is not set!");
        }
    }

    // Botão Settings
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // Ativa o painel de configurações
        }
        else
        {
            Debug.LogError("Settings panel is not assigned!");
        }
    }

    // Botão Quit
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Fecha o jogo (funciona apenas no build)
    }

    // Fecha as configurações
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Fecha o painel de configurações
        }
    }
}

