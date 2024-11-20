using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    
    public Slider Speed;
    public Slider Boost;
    public Slider Drift;
    public Slider Handling;

    
    public float speedValue = 50f;  
    public float boostValue = 70f;  
    public float driftValue = 60f;  
    public float handlingValue = 80f;  

    
    public void OnNextButtonClicked()
    {
        // Alterar os valores dos Sliders
        Speed.value = speedValue;
        Boost.value = boostValue;
        Drift.value = driftValue;
        Handling.value = handlingValue;
    }
}


