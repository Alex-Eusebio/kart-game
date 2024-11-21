using UnityEngine;

public class UICharacterAnimation : MonoBehaviour
{
    [SerializeField] private GameObject characterTarget;  // O Empty que define o destino das personagens
    private int currentCharacterIndex = 0; // Índice da personagem atual
    public GameObject[] characterModels; // Array com os modelos das personagens
    public float moveSpeed = 0.1f; // Velocidade de movimento

    void Start()
    {
        // Atualiza animações e posições logo no início
        UpdateCharacterAnimations();
    }

    // Função chamada ao clicar no botão 'Next'
    public void OnNextButtonClicked()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % characterModels.Length; // Avança para a próxima personagem
        UpdateCharacterAnimations(); // Atualiza animações e posições
    }

    // Função chamada ao clicar no botão 'Previous'
    public void OnPreviousButtonClicked()
    {
        currentCharacterIndex = (currentCharacterIndex - 1 + characterModels.Length) % characterModels.Length; // Volta para a anterior
        UpdateCharacterAnimations(); // Atualiza animações e posições
    }

    // Função que atualiza a posição da personagem para o CharacterTarget e toca a animação
    private void UpdateCharacterAnimations()
    {
        GameObject currentCharacter = characterModels[currentCharacterIndex]; // Obtém o modelo da personagem atual

        if (currentCharacter != null)
        {
            // Chama a função para mover a personagem para o CharacterTarget
            StartCoroutine(MoveToPosition(currentCharacter.transform, characterTarget.transform.position));

            // Ativa o Animator da personagem atual e toca a animação Idle
            Animator animator = currentCharacter.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
                animator.Play("Idle", 0, 0f); // Força a animação Idle a começar
            }
        }
    }

    // Corrotina que move a personagem suavemente até o destino
    private System.Collections.IEnumerator MoveToPosition(Transform characterTransform, Vector3 targetPosition)
    {
        // Enquanto a personagem não atingir a posição do CharacterTarget
        while (Vector3.Distance(characterTransform.position, targetPosition) > 0.1f)
        {
            // Movimenta a personagem para a posição do CharacterTarget de forma suave
            characterTransform.position = Vector3.Lerp(characterTransform.position, targetPosition, moveSpeed);
            yield return null; // Espera até o próximo frame
        }

        // Garante que a personagem chegue exatamente na posição do CharacterTarget
        characterTransform.position = targetPosition;
    }
}



