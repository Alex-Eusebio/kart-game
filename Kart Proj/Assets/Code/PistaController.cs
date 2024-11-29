using UnityEngine;

public class PistaController : MonoBehaviour
{
    [Header("Personagens")]
    public GameObject[] characterPrefabs;  // Referência para os prefabs dos personagens
    public Transform[] spawnPoints;        // Pontos de spawn para a personagem
    public Camera mainCamera;              // A câmera principal para seguir a personagem

    public DebugCanvas debugCanvas;

    private GameObject spawnedCharacter;  // Armazenará a personagem que foi spawnada
    private Transform characterTransform; // Referência para o transform da personagem para a câmera seguir

    private void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter");
        Debug.Log("Personagem escolhida na cena anterior: " + selectedCharacter); 
        
        // Instancia a personagem na cena
        InstantiateCharacter(selectedCharacter);

        // Ajusta a câmera para seguir a personagem
        if (spawnedCharacter != null)
            characterTransform = spawnedCharacter.transform;
    }

    // Método de escolha de personagem (baseado no PlayerPrefs)
    private void InstantiateCharacter(int characterId)
    {
        int i = 0;
        // Verifica o nome do personagem e instância o prefab correspondente
        foreach (var characterPrefab in characterPrefabs)
        {
            if (i == characterId)
            {
                int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[randomSpawnIndex];

                spawnedCharacter = Instantiate(characterPrefab, spawnPoint.position, spawnPoint.rotation);
                Debug.Log($"Personagem {characterPrefab.name} foi spawnada na posição {spawnPoint.position}");

                // Configurar a câmera para seguir a personagem
                CameraFollow cameraFollow = spawnedCharacter.GetComponentInChildren<CameraFollow>();
                if (cameraFollow != null)
                {
                    cameraFollow._camera = mainCamera;
                    Debug.Log("Câmera configurada para seguir a personagem.");
                }

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
}












