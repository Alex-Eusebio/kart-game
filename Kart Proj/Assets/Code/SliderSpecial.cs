using UnityEngine;
using UnityEngine.UI;

public class SliderSpecial : MonoBehaviour
{
    public CarSystem carSystem;           // Sistema associado
    public Slider[] specialSliders;      // Array de sliders associados

    private void Update()
    {
        if (specialSliders != null)
        {
            foreach (var specialSlider in specialSliders)
            {
                if (specialSlider != null)
                {
                    // Atualiza cada slider
                    specialSlider.value = carSystem.special.resource;
                    specialSlider.maxValue = carSystem.special.maxResource;
                }
            }
        }
    }
}
