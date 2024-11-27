using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    [Header("UI Components")]
    public Image[] characterImages;    // Imagens dos personagens
    public Image[] spriteDisplays;     // Sprites associados aos personagens
    public Image[] infoImages;         // Imagens de informações para cada personagem
    public Slider Speed;
    public Slider Boost;
    public Slider Drift;
    public Slider Handling;

    [Header("Image Sizes")]
    public Vector2 defaultSize = new Vector2(100f, 100f);
    public Vector2 enlargedSize = new Vector2(150f, 150f);
    public Vector2 pulseSize = new Vector2(120f, 120f);
    public float pulseSpeed = 0.9f;

    private int currentCharacterIndex = 0; // Índice do personagem atual
    private Coroutine pulseCoroutine;
    private Character[] characters;

    void Start()
    {
        // Inicializa os personagens e seus atributos
        characters = new Character[]
        {
            new Character("Ben", 50f, 60f, 70f, 80f),
            new Character("Max", 80f, 90f, 60f, 70f),
            new Character("Rato", 60f, 70f, 50f, 90f),
            new Character("Tanks", 90f, 50f, 75f, 65f),
            new Character("Zum", 70f, 85f, 80f, 55f)
        };

        SetSliderRange(0f, 100f); // Define os limites dos sliders
        UpdateUI();
        StartPulseEffect();
    }

    void Update()
    {
        // Controle de navegação usando teclas
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentCharacterIndex > 0)
        {
            ChangeCharacter(-1);
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentCharacterIndex < characters.Length - 1)
        {
            ChangeCharacter(1);
        }

        // Detecta quando o jogador pressiona Enter ou Espaço para avançar para a próxima cena
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            OnEnterButtonClicked();
        }
    }

    public void OnNextButtonClicked()
    {
        if (currentCharacterIndex < characters.Length - 1)
        {
            ChangeCharacter(1);
        }
    }

    public void OnPreviousButtonClicked()
    {
        if (currentCharacterIndex > 0)
        {
            ChangeCharacter(-1);
        }
    }

    private void ChangeCharacter(int direction)
    {
        StopPulseEffect();
        currentCharacterIndex += direction;
        UpdateUI();
        StartPulseEffect();
    }

    private void UpdateUI()
    {
        UpdateImageSizes();   // Atualiza os tamanhos das imagens dos personagens
        UpdateSpriteDisplay(); // Atualiza os sprites
        UpdateInfoImages();    // Atualiza as imagens de informações
        UpdateSliders();       // Atualiza os sliders
    }

    private void UpdateImageSizes()
    {
        for (int i = 0; i < characterImages.Length; i++)
        {
            characterImages[i].rectTransform.sizeDelta = (i == currentCharacterIndex) ? enlargedSize : defaultSize;
        }
    }

    private void UpdateSpriteDisplay()
    {
        for (int i = 0; i < spriteDisplays.Length; i++)
        {
            spriteDisplays[i].enabled = (i == currentCharacterIndex);
        }
    }

    private void UpdateInfoImages()
    {
        for (int i = 0; i < infoImages.Length; i++)
        {
            infoImages[i].enabled = (i == currentCharacterIndex);
        }
    }

    private void UpdateSliders()
    {
        Character currentCharacter = characters[currentCharacterIndex];
        Speed.value = currentCharacter.Speed;
        Boost.value = currentCharacter.Boost;
        Drift.value = currentCharacter.Drift;
        Handling.value = currentCharacter.Handling;
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
            yield return SmoothResize(characterImages[currentCharacterIndex].rectTransform, enlargedSize, pulseSize, pulseSpeed);
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
        target.sizeDelta = endSize;
    }

    private void SetSliderRange(float minValue, float maxValue)
    {
        Speed.minValue = minValue;
        Speed.maxValue = maxValue;
        Boost.minValue = minValue;
        Boost.maxValue = maxValue;
        Drift.minValue = minValue;
        Drift.maxValue = maxValue;
        Handling.minValue = minValue;
        Handling.maxValue = maxValue;
    }

    // Método chamado quando o jogador clica Enter ou Espaço
    public void OnEnterButtonClicked()
    {
        // Salva o nome da personagem escolhida no PlayerPrefs
        PlayerPrefs.SetString("SelectedCharacter", characters[currentCharacterIndex].Name);
        PlayerPrefs.Save();  // Garante que o valor seja salvo
        Debug.Log("Personagem escolhida: " + characters[currentCharacterIndex].Name);
        // Aqui você pode adicionar o código para avançar para a próxima cena
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage Select");
    }
}

[System.Serializable]
public class Character
{
    public string Name;
    public float Speed;
    public float Boost;
    public float Drift;
    public float Handling;

    public Character(string name, float speed, float boost, float drift, float handling)
    {
        Name = name;
        Speed = speed;
        Boost = boost;
        Drift = drift;
        Handling = handling;
    }
}










