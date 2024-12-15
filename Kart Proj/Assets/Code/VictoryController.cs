using UnityEngine;

public class VictoryController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject[] characters;  // Array de personagens na cena
    public Transform[] spawnPoints; // Pontos de spawn

    private bool raceFinished = false;    // Controle se a corrida terminou

    public void FinishRace()
    {
        if (!raceFinished)
        {
            raceFinished = true;
            SpawnCharacters();
        }
    }

    private void SpawnCharacters()
    {
        if (characters.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Não há personagens ou pontos de spawn configurados!");
            return;
        }

        // Ativa cada personagem e posiciona nos pontos de spawn
        for (int i = 0; i < characters.Length && i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            GameObject character = characters[i];

            // Posiciona e ativa a personagem
            character.transform.position = spawnPoint.position;
            character.transform.rotation = spawnPoint.rotation;
            character.SetActive(true);

            Debug.Log($"Personagem {character.name} foi ativada na posição {spawnPoint.position}");

            // Dispara a animação de vitória
            Animator animator = character.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Victory"); // Trigger configurado no Animator Controller
            }
            else
            {
                Debug.LogWarning($"Nenhum Animator encontrado em {character.name}");
            }
        }
    }
}
