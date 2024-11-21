using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider Speed;
    public Slider Boost;
    public Slider Drift;
    public Slider Handling;

    private Character[] characters;
    private int currentCharacterIndex = 0;

    void Start()
    {
        characters = new Character[]
        {
            new Character("Ben", 50f, 60f, 70f, 80f),
            new Character("Max", 80f, 90f, 60f, 70f),
            new Character("Rato", 60f, 70f, 50f, 90f),
            new Character("Tanks", 90f, 50f, 75f, 65f),
            new Character("Zum", 70f, 85f, 80f, 55f)
        };

        SetSliderRange(0f, 100f);

        UpdateSliders();
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

    void Update()
    {
        // Verifica a entrada das teclas A e D ou as setas para controlar a mudança de personagem
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentCharacterIndex > 0)
        {
            OnPreviousButtonClicked();
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentCharacterIndex < characters.Length - 1)
        {
            OnNextButtonClicked();
        }
    }

    public void OnNextButtonClicked()
    {
        // Avança para o próximo personagem se não for o último
        if (currentCharacterIndex < characters.Length - 1)
        {
            currentCharacterIndex++;
            UpdateSliders();
        }
    }

    public void OnPreviousButtonClicked()
    {
        // Volta para o personagem anterior se não for o primeiro
        if (currentCharacterIndex > 0)
        {
            currentCharacterIndex--;
            UpdateSliders();
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





