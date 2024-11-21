using UnityEngine;

public class CharacterSelectIdleAnimation : MonoBehaviour
{
    public GameObject[] characterModels; // Array com os modelos das personagens
    private int currentCharacterIndex = 0; // Índice da personagem atual

    void Start()
    {
        // Ao iniciar, garanta que todas as animações Idle comecem
        UpdateCharacterAnimations();
    }

    public void OnNextButtonClicked()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % characterModels.Length;
        UpdateCharacterAnimations();
    }

    public void OnPreviousButtonClicked()
    {
        currentCharacterIndex = (currentCharacterIndex - 1 + characterModels.Length) % characterModels.Length;
        UpdateCharacterAnimations();
    }

    private void UpdateCharacterAnimations()
    {
        // Itera por todas as personagens
        for (int i = 0; i < characterModels.Length; i++)
        {
            Animator animator = characterModels[i].GetComponent<Animator>();

            if (animator != null)
            {
                // Se a personagem for a atual, forçar a animação Idle
                if (i == currentCharacterIndex)
                {
                    animator.enabled = true; // Garante que o Animator está ativo
                    animator.Play("Idle", 0, 0f); // Força a animação Idle a tocar
                    Debug.Log(characterModels[i].name + " Animator Ativado e Idle Iniciado");
                }
                else
                {
                    animator.enabled = true; // Garante que os outros animadores também estão ativos
                    animator.Play("Idle", 0, 0f); // Garantir que todos estão tocando Idle também
                }
            }
        }
    }
}




