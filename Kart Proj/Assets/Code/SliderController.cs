using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    // Referências aos Sliders
    public Slider Speed;
    public Slider Boost;
    public Slider Drift;
    public Slider Handling;

    // Armazenamento das personagens e o índice da personagem atual
    private Character[] characters;
    private int currentCharacterIndex = 0;

    void Start()
    {
        // Inicializar as 5 personagens com atributos diferentes
        characters = new Character[]
        {
            new Character("Ben", 50f, 60f, 70f, 80f),
            new Character("Max", 80f, 90f, 60f, 70f),
            new Character("Rato", 60f, 70f, 50f, 90f),
            new Character("Tanks", 90f, 50f, 75f, 65f),
            new Character("Zum", 70f, 85f, 80f, 55f)
        };

        // Configurar os Sliders para o intervalo de 0 a 100
        SetSliderRange(0f, 100f);

        // Atualizar a UI com os atributos da primeira personagem
        UpdateSliders();
    }

    // Função para configurar os intervalos dos Sliders
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

    // Função chamada ao clicar no botão Next
    public void OnNextButtonClicked()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Length;
        UpdateSliders();
    }

    // Função chamada ao clicar no botão Previous
    public void OnPreviousButtonClicked()
    {
        currentCharacterIndex = (currentCharacterIndex - 1 + characters.Length) % characters.Length;
        UpdateSliders();
    }

    // Atualiza os Sliders com os atributos da personagem atual
    private void UpdateSliders()
    {
        Character currentCharacter = characters[currentCharacterIndex];

        // Atualizar os valores dos Sliders
        Speed.value = currentCharacter.Speed;
        Boost.value = currentCharacter.Boost;
        Drift.value = currentCharacter.Drift;
        Handling.value = currentCharacter.Handling;
    }
}

// Classe para armazenar os dados dos atributos de cada personagem
[System.Serializable]
public class Character
{
    public string Name;
    public float Speed;
    public float Boost;
    public float Drift;
    public float Handling;

    // Construtor para inicializar a personagem com valores
    public Character(string name, float speed, float boost, float drift, float handling)
    {
        Name = name;
        Speed = speed;
        Boost = boost;
        Drift = drift;
        Handling = handling;
    }
}





