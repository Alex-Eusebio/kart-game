using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CharacterSelectController : MonoBehaviour
{
    [SerializeField]
    private int players = 1; // Quantos players vao jogar
    [SerializeField] private int curPlayer;
    [SerializeField]
    private int currentCharacter = 0; // Índice do personagem atual
    [SerializeField]
    private Color redOut;
    [SerializeField]
    private Character[] characters;
    [Header("UI Components")]
    public Slider Speed;
    public Slider Boost;
    public Slider Drift;
    public Slider Handling;
    [SerializeField]
    private TextMeshProUGUI nameTxt;
    [SerializeField]
    private TextMeshProUGUI titleTxt;
    [SerializeField]
    private TextMeshProUGUI descTxt;
    [SerializeField]
    private TextMeshProUGUI specialNameTxt;
    [SerializeField]
    private Image specialIcon;
    [SerializeField]
    private Image backgroundMat;
    [SerializeField]
    private Sprite[] pick;

    [Header("Image Sizes")]
    public Vector2 defaultSize = new Vector2(100f, 100f);
    public Vector2 enlargedSize = new Vector2(150f, 150f);
    public Vector2 pulseSize = new Vector2(120f, 120f);
    public float pulseSpeed = 0.9f;

    private Coroutine pulseCoroutine;
    [SerializeField]
    List<int> pickedNumbers = new List<int>();

    void Start()
    {
        PlayerPrefs.DeleteKey("SelectedCharacter0");
        PlayerPrefs.DeleteKey("SelectedCharacter1");
        PlayerPrefs.DeleteKey("SelectedCharacter2");
        PlayerPrefs.DeleteKey("SelectedCharacter3");

        if (PlayerPrefs.HasKey("PlayerCount"))
            players = PlayerPrefs.GetInt("PlayerCount");

        UpdateUI();
        StartPulseEffect(); 
        ChangeCharacter(0); // Seleciona a primeira personagem no início
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            ChangeCharacter(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            ChangeCharacter(1);

        // Detecta quando o jogador pressiona Enter ou Espaço para avançar para a próxima cena
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            OnEnterButtonClicked();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PlayerPrefs.HasKey("IsAi"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            else
            {
                PlayerPrefs.DeleteKey("IsAi");
                SceneManager.LoadScene("MainMenu");
            }
        }
        
    }

    private void ChangeCharacter(int _change)
    {
        StopPulseEffect();

        currentCharacter += _change;

        if (currentCharacter < 0)
            currentCharacter = characters.Length - 1;
        else if (currentCharacter > characters.Length - 1)
            currentCharacter = 0;

        if (curPlayer > 0)
        {
            while (pickedNumbers.Contains(currentCharacter))
            {
                currentCharacter += _change;

                if (currentCharacter < 0)
                    currentCharacter = characters.Length - 1;
                else if (currentCharacter > characters.Length - 1)
                    currentCharacter = 0;
            }
        }

        // Ativar apenas o personagem atual e desativar os outros
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].charcModel.SetActive(i == currentCharacter);
            characters[i].carModel.SetActive(i == currentCharacter);
        }

        UpdateUI();
        StartPulseEffect();
    }

    private void UpdateUI()
    {
        backgroundMat.material.SetColor("_Color", characters[currentCharacter].backgroundColor);

        nameTxt.text = characters[currentCharacter].name;
        titleTxt.text = characters[currentCharacter].title;
        descTxt.text = characters[currentCharacter].specialDescription;
        specialNameTxt.text = characters[currentCharacter].specialName;
        specialIcon.sprite = characters[currentCharacter].specialIcon;
        characters[currentCharacter].unselectedDisplay.gameObject.SetActive(false);
        characters[currentCharacter].spriteDisplay.gameObject.SetActive(true);
        characters[currentCharacter].spriteDisplay.rectTransform.sizeDelta = enlargedSize;

        for (int i = 0; i < characters.Length; i++)
        {
            if (i != currentCharacter)
            {
                characters[i].spriteDisplay.rectTransform.sizeDelta = defaultSize;
                characters[i].spriteDisplay.gameObject.SetActive(false);
                characters[i].unselectedDisplay.gameObject.SetActive(true);
            }
        }

        UpdateSliders();       // Atualiza os sliders
    }

    private void UpdateSliders()
    {
        Speed.value = characters[currentCharacter].speed;
        Boost.value = characters[currentCharacter].boost;
        Drift.value = characters[currentCharacter].drift;
        Handling.value = characters[currentCharacter].handling;
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
            yield return SmoothResize(characters[currentCharacter].spriteDisplay.rectTransform, enlargedSize, pulseSize, pulseSpeed);
            yield return SmoothResize(characters[currentCharacter].spriteDisplay.rectTransform, pulseSize, enlargedSize, pulseSpeed);
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

    // Método chamado quando o jogador clica Enter ou Espaço
    public void OnEnterButtonClicked()
    {
        // Salva o nome da personagem escolhida no PlayerPrefs
        PlayerPrefs.SetInt("SelectedCharacter" + curPlayer, currentCharacter);
        PlayerPrefs.Save();  // Garante que o valor seja salvo
        Debug.Log("Personagem escolhida: " + characters[currentCharacter].name);
        curPlayer++;

        if (curPlayer == players)
        {
            if (PlayerPrefs.HasKey("IsAi"))
                UnityEngine.SceneManagement.SceneManager.LoadScene("Pista AI");
            else 
                UnityEngine.SceneManagement.SceneManager.LoadScene("Stage Select");

            PlayerPrefs.DeleteKey("IsAi");
        }

        characters[currentCharacter].pickNumber.gameObject.SetActive(true);
        characters[currentCharacter].pickNumber.sprite = pick[curPlayer-1];
        characters[currentCharacter].unselectedDisplay.color = redOut;
        pickedNumbers.Add(currentCharacter);
        ChangeCharacter(1);
    }
}

[System.Serializable]
public struct Character
{
    public string name;
    public string title;
    public Color backgroundColor;
    public Sprite specialIcon;
    public string specialName;
    [TextArea(3, 3)]
    public string specialDescription;
    [Range(0.0f, 100.0f)]
    public float speed;
    [Range(0.0f, 100.0f)]
    public float boost;
    [Range(0.0f, 100.0f)]
    public float drift;
    [Range(0.0f, 100.0f)]
    public float handling;
    public GameObject charcModel;
    public GameObject carModel;
    public Image spriteDisplay;     // Sprites associados aos personagens
    public Image unselectedDisplay;         // Imagens de informações para cada personagem
    public Image pickNumber;
}










