using UnityEngine;
using UnityEngine.UI;

public class PistaController : MonoBehaviour
{
    [Header("Personagens")]
    public GameObject[] characterPrefabs;  // Prefabs dos personagens
    public Transform[] spawnPoints;        // Pontos de spawn

    public DebugCanvas debugCanvas;        // Debug Canvas (opcional)
    public Slider[] characterSliders;      // Sliders no Canvas (um para cada personagem)

    private GameObject spawnedCharacter;   // Referência para o personagem spawnado

    public SliderSpecial sliderSpecial; //Referencia ao sliderSpawner

    private void Start()
    {
        int i = 0;

        while (true)
        {
            int selectedCharacter = 0;
            if (PlayerPrefs.HasKey("SelectedCharacter" + i))
            {
                selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter" + i);
                Debug.Log(i);
            }
            else
                break;

            Debug.Log($"Personagem escolhida na cena anterior: {selectedCharacter}");

            // Instancia a personagem
            InstantiateCharacter(selectedCharacter);

            // Ativar o slider correspondente
            ActivateCharacterSlider(selectedCharacter);

            //sliderSpecial.carSystem = spawnedCharacter.GetComponentInChildren<CarSystem>();
            i++;
        }    
    }

    private void InstantiateCharacter(int characterId)
    {
        int i = 0;
        foreach (var characterPrefab in characterPrefabs)
        {
            if (i == characterId)
            {
                int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[randomSpawnIndex];

                spawnedCharacter = Instantiate(characterPrefab, spawnPoint.position, spawnPoint.rotation);
                Debug.Log($"Personagem {characterPrefab.name} foi spawnada na posição {spawnPoint.position}");

                // Associar o sistema de carro ao DebugCanvas
                if (debugCanvas != null)
                {
                    debugCanvas.SetCar(spawnedCharacter.GetComponentInChildren<CarSystem>());
                    Debug.Log("Carro associado ao DebugCanvas.");
                }

                break;
            }
            i++;
        }
    }

    private void ActivateCharacterSlider(int characterId)
    {
        Debug.Log("Iniciando ativação dos sliders...");
        
        // Desativa todos os sliders primeiro
        for (int i = 0; i < characterSliders.Length; i++)
        {
            characterSliders[i].gameObject.SetActive(false);
            Debug.Log($"Slider {i} desativado.");
        }

        // Ativa apenas o slider correspondente ao personagem escolhido
        if (characterId >= 0 && characterId < characterSliders.Length)
        {
            characterSliders[characterId].gameObject.SetActive(true);
            Debug.Log($"Slider {characterId} ativado para o personagem correspondente.");
        }
        else
        {
            Debug.LogWarning("ID do personagem fora do intervalo dos sliders.");
        }
    }
}












