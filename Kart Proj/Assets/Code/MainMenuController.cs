using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;  // Necessário para usar corrotinas

public class MainMenuController : MonoBehaviour
{
    public Button[] menuButtons; // Array de botões no menu
    private int currentIndex = 0; // Índice do botão atualmente selecionado
    public int playSceneName; // Nome da cena para o botão Play
    public int playAiScene; // Nome da cena para o botão Play
    public int settingsSceneName; // Nome da cena para o botão Settings
    public int creditsSceneName; // Nome da cena para o botão Settings

    private Vector3[] defaultScales; // Escalas padrões dos botões
    private float scalePulse = 1.1f; // Fator de escala para o efeito de "pulsar"
    private float pulseSpeed = 1.5f; // Velocidade de pulsação

    private Coroutine pulseCoroutine = null; // Corrotina para pulsação

    void Start()
    {
        if (menuButtons.Length > 0)
        {
            defaultScales = new Vector3[menuButtons.Length];
            
            // Salva a escala padrão de cada botão
            for (int i = 0; i < menuButtons.Length; i++)
            {
                defaultScales[i] = menuButtons[i].transform.localScale;
            }

            HighlightButton(currentIndex); // Inicia destacando o primeiro botão
        }
    }

    void Update()
    {
        HandleInput(); // Gerencia as entradas do jogador
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
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

    // Destaca o botão selecionado e começa a pulsação
    private void HighlightButton(int index)
    {
        if (menuButtons[index] != null)
        {
            menuButtons[index].Select(); // Destaca o botão
            menuButtons[index].transform.localScale = defaultScales[index] * scalePulse; // Aumenta a escala do botão selecionado

            // Se a corrotina não estiver rodando, inicia a pulsação
            if (pulseCoroutine == null)
            {
                pulseCoroutine = StartCoroutine(PulseButton(menuButtons[index])); // Inicia a pulsação do botão
            }
        }
    }

    // Remove o destaque do botão e para a pulsação
    private void RemoveHighlight(int index)
    {
        if (menuButtons[index] != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // Remove o foco
            menuButtons[index].transform.localScale = defaultScales[index]; // Restaura a escala padrão do botão

            // Para a corrotina de pulsação se ela estiver rodando
            if (pulseCoroutine != null)
            {
                StopCoroutine(pulseCoroutine); // Para a pulsação
                pulseCoroutine = null;
            }
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

    // Corrotina para pulsar o botão
    private IEnumerator PulseButton(Button button)
    {
        while (true)
        {
            // Pulsação para aumentar
            float time = Mathf.PingPong(Time.time * pulseSpeed, 1);
            button.transform.localScale = defaultScales[currentIndex] * (scalePulse + time * 0.1f); // 0.1f de variação

            yield return null;
        }
    }

    // Botão Play
    public void PlayGame()
    {
        SceneManager.LoadScene(playSceneName); // Carrega a cena do jogo
    }

    // Botão Play
    public void PlayAi()
    {
        PlayerPrefs.SetInt("PlayerCount", 1);
        PlayerPrefs.SetInt("IsAi", 1);
        SceneManager.LoadScene(playAiScene); // Carrega a cena do jogo
    }

    // Botão Settings (agora carrega uma nova cena)
    public void OpenSettings()
    {
        SceneManager.LoadScene(settingsSceneName); // Carrega a cena de configurações
    }

    // Botão Credits (agora carrega uma nova cena)
    public void OpenCredits()
    {
        SceneManager.LoadScene(creditsSceneName); // Carrega a cena de configurações
    }

    // Botão Quit
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Fecha o jogo (funciona apenas no build)
    }
}





