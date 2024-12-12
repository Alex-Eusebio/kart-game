using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PistaController : MonoBehaviour
{
    [Header("Personagens")]
    public GameObject[] characterPrefabs;  // Prefabs dos personagens
    public Transform[] spawnPoints;        // Pontos de spawn

    private void Start()
    {
        int i = 0;

        while (true)
        {
            int selectedCharacter = 0;
            if (PlayerPrefs.HasKey("SelectedCharacter" + i))
            {
                selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter" + i);
            }
            else
                break;

            Debug.Log($"Personagem escolhida na cena anterior: {selectedCharacter}");

            // Instancia a personagem
            InstantiateCharacter(selectedCharacter);

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

                GameObject spawnedCharacter = Instantiate(characterPrefab, spawnPoint.position, spawnPoint.rotation);
                Debug.Log($"Personagem {characterPrefab.name} foi spawnada na posição {spawnPoint.position}");

                break;
            }
            i++;
        }
    }
}












