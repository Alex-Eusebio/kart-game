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
        // Forçando o spawn do primeiro prefab no primeiro ponto de spawn para testar
        if (characterPrefabs.Length > 0 && spawnPoints.Length > 0)
        {
            // Força o spawn do primeiro prefab no primeiro ponto de spawn
            spawnedCharacter = Instantiate(characterPrefabs[0], spawnPoints[0].position, spawnPoints[0].rotation);
            Debug.Log($"Prefab instanciado manualmente: {spawnedCharacter.name} na posição {spawnPoints[0].position}");

            // Verificar a escala do personagem
            Debug.Log($"Escala do personagem instanciado: {spawnedCharacter.transform.localScale}");

            // Garantir que o personagem está ativo
            if (!spawnedCharacter.activeInHierarchy)
            {
                spawnedCharacter.SetActive(true);
                Debug.Log("O personagem foi desativado, mas agora foi ativado.");
            }

            // Verificar a posição final do personagem
            Debug.Log($"Posição final do personagem: {spawnedCharacter.transform.position}");

            // Ajustar a câmera para seguir a personagem
            if (spawnedCharacter != null)
            {
                characterTransform = spawnedCharacter.transform;
                // Atribuindo a câmera para o script de seguimento (assumindo que CameraFollow existe)
                CameraFollow cameraFollow = spawnedCharacter.GetComponentInChildren<CameraFollow>();
                if (cameraFollow != null)
                {
                    cameraFollow._camera = mainCamera;
                    Debug.Log("Câmera configurada para seguir a personagem.");
                }
                else
                {
                    Debug.LogError("Componente CameraFollow não encontrado no prefab da personagem.");
                }

                // Conectar o sistema do carro ao DebugCanvas
                if (debugCanvas != null)
                {
                    CarSystem carSystem = spawnedCharacter.GetComponentInChildren<CarSystem>();
                    if (carSystem != null)
                    {
                        debugCanvas.SetCar(carSystem);
                        Debug.Log("Carro associado ao DebugCanvas.");
                    }
                    else
                    {
                        Debug.LogError("CarSystem não encontrado na personagem.");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Nenhum prefab ou ponto de spawn configurado!");
        }
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












