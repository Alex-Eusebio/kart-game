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

    public float pulseSpeed = 0.9f; // Velocidade ligeiramente reduzida


    private Coroutine pulseCoroutine;

    void Start()
    {
        UpdateImageSizes();
        StartPulseEffect();
    }

    public void OnNextButtonClicked()
    {
        StopPulseEffect();
        currentCharacterIndex = (currentCharacterIndex + 1) % characterImages.Length;
        UpdateImageSizes();
        StartPulseEffect();
    }

    public void OnPreviousButtonClicked()
    {
        StopPulseEffect();
        currentCharacterIndex = (currentCharacterIndex - 1 + characterImages.Length) % characterImages.Length;
        UpdateImageSizes();
        StartPulseEffect();
    }

    private void UpdateImageSizes()
    {
        for (int i = 0; i < characterImages.Length; i++)
        {
            characterImages[i].rectTransform.sizeDelta = (i == currentCharacterIndex) ? enlargedSize : defaultSize;
        }
    }

    private void StartPulseEffect()
    {
        pulseCoroutine = StartCoroutine(PulseEffect());
    }

    private void StopPulseEffect()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
        }
    }

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



