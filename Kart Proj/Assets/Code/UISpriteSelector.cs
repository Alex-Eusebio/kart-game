using UnityEngine;
using UnityEngine.UI;

public class UISpriteSelector : MonoBehaviour
{
    public Image[] spriteDisplays; 
    private int currentImageIndex = 0; 

    void Start()
    {
        
        UpdateImageDisplay();
    }

    
    void UpdateImageDisplay()
    {
        
        foreach (Image img in spriteDisplays)
        {
            img.enabled = false;
        }

        
        if (spriteDisplays.Length > 0)
        {
            spriteDisplays[currentImageIndex].enabled = true;
        }
    }

    
    public void NextSprite()
    {
        
        currentImageIndex = (currentImageIndex + 1) % spriteDisplays.Length;
        UpdateImageDisplay(); 
    }

    
    public void PreviousSprite()
    {
        
        currentImageIndex = (currentImageIndex - 1 + spriteDisplays.Length) % spriteDisplays.Length;
        UpdateImageDisplay(); 
    }
}





