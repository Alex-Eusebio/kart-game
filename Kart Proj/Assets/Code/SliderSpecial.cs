using UnityEngine;
using UnityEngine.UI;

public class SliderSpecial : MonoBehaviour
{
    public CarSystem carSystem;
    public Slider specialSlider; // O slider associado a este personagem

    // Método para atualizar a barra (você pode adicionar sua lógica aqui)
    private void Update()
    {
        if (specialSlider != null)
        {
            specialSlider.value = carSystem.special.resource;
            specialSlider.maxValue = carSystem.special.maxResource;
        }
    }
}
