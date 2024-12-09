using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectorMenu : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> menuItems; // Lista de itens do menu
    private int currentIndex = 0; // Índice do item selecionado atualmente
    [SerializeField]
    private Color selectedColor = Color.yellow; // Cor para o item selecionado
    [SerializeField]
    private Color normalColor = Color.white; // Cor para itens não selecionados
    [SerializeField]
    private Vector3 normalScale = Vector3.one; // Escala normal
    [SerializeField]
    private Vector3 pulseScale = new Vector3(1.2f, 1.2f, 1.2f); // Escala durante a pulsação
    [SerializeField]
    private float pulseSpeed = 0.5f; // Velocidade da pulsação

    private Coroutine pulseCoroutine;

    private void Start()
    {
        PlayerPrefs.DeleteKey("PlayerCount");
        UpdateMenuVisuals();
        StartPulseEffect();
    }

    private void Update()
    {
        // Navegar para a esquerda
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChangeSelection(-1);
        }
        // Navegar para a direita
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeSelection(1);
        }

        // Confirmar seleção
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmSelection();
        }

        // Voltar para a cena anterior com Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
        }
    }

    private void ChangeSelection(int change)
    {
        // Para a animação do item atualmente selecionado
        StopPulseEffect();

        currentIndex += change;

        // Garantir que o índice esteja dentro dos limites
        if (currentIndex < 0)
            currentIndex = menuItems.Count - 1;
        else if (currentIndex >= menuItems.Count)
            currentIndex = 0;

        // Atualiza as visuais do menu e inicia a animação do novo item selecionado
        UpdateMenuVisuals();
        StartPulseEffect();
    }

    private void UpdateMenuVisuals()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            // Atualiza as cores dos itens de menu
            var renderer = menuItems[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = (i == currentIndex) ? selectedColor : normalColor;
            }

            // Atualiza a escala para itens não selecionados
            menuItems[i].transform.localScale = (i == currentIndex) ? pulseScale : normalScale;
        }
    }

    private void StartPulseEffect()
    {
        // Inicia a animação de pulsação para o item selecionado
        pulseCoroutine = StartCoroutine(PulseEffect(menuItems[currentIndex]));
    }

    private void StopPulseEffect()
    {
        // Para a animação de pulsação
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }

        // Restaura o tamanho normal do item
        menuItems[currentIndex].transform.localScale = normalScale;
    }

    private IEnumerator PulseEffect(GameObject target)
    {
        Transform targetTransform = target.transform;

        while (true)
        {
            // Expande o item para o tamanho de pulsação
            yield return SmoothScale(targetTransform, normalScale, pulseScale, pulseSpeed);

            // Volta o item para o tamanho normal
            yield return SmoothScale(targetTransform, pulseScale, normalScale, pulseSpeed);
        }
    }

    private IEnumerator SmoothScale(Transform target, Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            target.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        target.localScale = endScale;
    }

    private void ConfirmSelection()
    {
        Debug.Log($"Jogadores Selecionados: {currentIndex + 1}");
        SelectPlayer(currentIndex + 1); // Passa o número de jogadores selecionados
    }

    public void SelectPlayer(int i)
    {
        PlayerPrefs.SetInt("PlayerCount", i);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}


