using UnityEngine;
using UnityEngine.UI;

public class SliderSpecial : MonoBehaviour
{
    public Slider specialSlider; // O slider associado a este personagem

    // Método para atualizar a barra (você pode adicionar sua lógica aqui)
    public void UpdateSlider(float value)
    {
        if (specialSlider != null)
        {
            specialSlider.value = value;
        }
    }
}
