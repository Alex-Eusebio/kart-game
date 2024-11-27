using UnityEngine;
using UnityEngine.SceneManagement; // Para carregar a próxima cena, caso necessário

public class PressAnyKeyToStart : MonoBehaviour
{
    public string nextSceneName; // Nome da próxima cena (opcional, se você usar transição de cena)
    
    private bool keyPressed = false; // Garante que o evento só aconteça uma vez

    void Update()
    {
        if (!keyPressed && Input.anyKeyDown) // Detecta se qualquer tecla foi pressionada
        {
            keyPressed = true; // Impede múltiplas execuções

            // Chamar uma ação ou carregar a próxima cena
            OnStartGame();
        }
    }

    private void OnStartGame()
    {
        Debug.Log("Starting game...");

        // Se nextSceneName estiver configurado, carregue a próxima cena
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Caso contrário, faça algo diferente aqui (ex.: desativar o texto ou iniciar animações)
            gameObject.SetActive(false); // Oculta o texto
        }
    }
}
