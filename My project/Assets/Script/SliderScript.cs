using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour
{
    private Slider slider;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 100f;
    }
    public void Slider_Update(float var)
    {
        slider.value = var;
    }
}
