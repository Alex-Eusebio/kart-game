using UnityEngine;
using UnityEngine.UI;

public class UISpriteSelector : MonoBehaviour
{
    public Image[] spriteDisplays; // Array de imagens para os sprites
    private int currentImageIndex = 0; // Índice da imagem atual

    void Start()
    {
        UpdateImageDisplay();
    }

    void Update()
    {
        // Verifica entrada das teclas com limites
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentImageIndex > 0)
        {
            PreviousSprite();
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentImageIndex < spriteDisplays.Length - 1)
        {
            NextSprite();
        }
    }

    void UpdateImageDisplay()
    {
        // Desativa todas as imagens
        foreach (Image img in spriteDisplays)
        {
            img.enabled = false;
        }

        // Ativa a imagem correspondente ao índice atual
        if (spriteDisplays.Length > 0)
        {
            spriteDisplays[currentImageIndex].enabled = true;
        }
    }

    public void NextSprite()
    {
        // Avança para o próximo sprite se não for o último
        if (currentImageIndex < spriteDisplays.Length - 1)
        {
            currentImageIndex++;
            UpdateImageDisplay();
        }
    }

    public void PreviousSprite()
    {
        // Volta para o sprite anterior se não for o primeiro
        if (currentImageIndex > 0)
        {
            currentImageIndex--;
            UpdateImageDisplay();
        }
    }
}







