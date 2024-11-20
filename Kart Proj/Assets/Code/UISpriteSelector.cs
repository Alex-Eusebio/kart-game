using UnityEngine;
using UnityEngine.UI;

public class UISpriteSelector : MonoBehaviour
{
    public Image[] spriteDisplays; // Array de Imagens que queremos alternar.
    private int currentImageIndex = 0; // Índice da imagem atual.

    void Start()
    {
        // Configura a imagem inicial quando o jogo começa
        UpdateImageDisplay();
    }

    // Atualiza a imagem visível
    void UpdateImageDisplay()
    {
        // Torna todas as imagens invisíveis
        foreach (Image img in spriteDisplays)
        {
            img.enabled = false;
        }

        // Torna visível a imagem atual
        if (spriteDisplays.Length > 0)
        {
            spriteDisplays[currentImageIndex].enabled = true;
        }
    }

    // Método para avançar para a próxima imagem
    public void NextSprite()
    {
        // Avança para a próxima imagem (looping ao chegar no fim)
        currentImageIndex = (currentImageIndex + 1) % spriteDisplays.Length;
        UpdateImageDisplay(); // Atualiza as imagens visíveis
    }

    // Método para voltar para a imagem anterior
    public void PreviousSprite()
    {
        // Volta para a imagem anterior (looping ao chegar no início)
        currentImageIndex = (currentImageIndex - 1 + spriteDisplays.Length) % spriteDisplays.Length;
        UpdateImageDisplay(); // Atualiza as imagens visíveis
    }
}



