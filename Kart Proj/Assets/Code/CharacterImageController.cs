using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterImageController : MonoBehaviour
{
    public Image[] characterImages; // Lista de imagens das personagens
    private int currentCharacterIndex = 0; // Índice da personagem ativa

    // Tamanhos das imagens
    public Vector2 defaultSize = new Vector2(100f, 100f);
    public Vector2 enlargedSize = new Vector2(150f, 150f);
    public Vector2 pulseSize = new Vector2(120f, 120f); // Tamanho intermediário para pulsação

    public float pulseSpeed = 0.9f; // Velocidade da pulsação

    private Coroutine pulseCoroutine;

    void Start()
    {
        UpdateImageSizes();
        StartPulseEffect();
    }

    void Update()
    {
        // Detecta teclas pressionadas e muda o personagem, mas verifica os limites
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentCharacterIndex > 0)
        {
            ChangeCharacter(-1); // Vai para o anterior
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentCharacterIndex < characterImages.Length - 1)
        {
            ChangeCharacter(1); // Vai para o próximo
        }
    }

    // Chamado pelos botões
    public void OnNextCharacter()
    {
        if (currentCharacterIndex < characterImages.Length - 1)
        {
            ChangeCharacter(1);
        }
    }

    public void OnPreviousCharacter()
    {
        if (currentCharacterIndex > 0)
        {
            ChangeCharacter(-1);
        }
    }

    // Método principal para alternar o personagem
    private void ChangeCharacter(int direction)
    {
        StopPulseEffect(); // Para o efeito de pulsação

        currentCharacterIndex += direction; // Atualiza o índice

        UpdateImageSizes(); // Atualiza os tamanhos das imagens
        StartPulseEffect(); // Reinicia o efeito de pulsação
    }

    // Atualiza os tamanhos das imagens (enlarged para o ativo, default para os outros)
    private void UpdateImageSizes()
    {
        for (int i = 0; i < characterImages.Length; i++)
        {
            characterImages[i].rectTransform.sizeDelta = (i == currentCharacterIndex) ? enlargedSize : defaultSize;
        }
    }

    // Inicia a pulsação da imagem ativa
    private void StartPulseEffect()
    {
        pulseCoroutine = StartCoroutine(PulseEffect());
    }

    // Para o efeito de pulsação
    private void StopPulseEffect()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
        }
    }

    // Efeito de pulsação
    private IEnumerator PulseEffect()
    {
        while (true)
        {
            // Reduzir tamanho para o pulso
            yield return SmoothResize(characterImages[currentCharacterIndex].rectTransform, enlargedSize, pulseSize, pulseSpeed);
            // Voltar ao tamanho original ampliado
            yield return SmoothResize(characterImages[currentCharacterIndex].rectTransform, pulseSize, enlargedSize, pulseSpeed);
        }
    }

    // Animação de redimensionamento suave
    private IEnumerator SmoothResize(RectTransform target, Vector2 startSize, Vector2 endSize, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            target.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            yield return null;
        }

        target.sizeDelta = endSize; // Garantir que o tamanho final é alcançado
    }
}








