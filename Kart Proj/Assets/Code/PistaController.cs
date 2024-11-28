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
        // Recupera o nome da personagem salva no PlayerPrefs
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter");
        Debug.Log("Personagem escolhida na cena anterior: " + selectedCharacter);

        // Instancia a personagem na cena
        InstantiateCharacter(selectedCharacter);

        // Ajusta a câmera para seguir a personagem
        if (spawnedCharacter != null)
        {
            characterTransform = spawnedCharacter.transform;
        }
    }


    private void InstantiateCharacter(string characterName)
    {
        // Tenta encontrar o personagem correspondente ao nome escolhido
        foreach (var characterPrefab in characterPrefabs)
        {
            // Verifica se o nome do prefab corresponde ao nome da personagem escolhida
            if (characterPrefab.name == characterName)
            {
                // Seleciona um ponto aleatório de spawn
                int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[randomSpawnIndex];

                // Instancia a personagem na posição do ponto de spawn
                spawnedCharacter = Instantiate(characterPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedCharacter.GetComponentInChildren <CameraFollow>()._camera=mainCamera;
                debugCanvas.SetCar(spawnedCharacter.GetComponentInChildren<CarSystem>());

                // Exibe no console que a personagem foi spawnada
                Debug.Log("Personagem " + characterName + " foi spawnada na posição " + spawnPoint.position);
                break;
            }
        }
    }
}













