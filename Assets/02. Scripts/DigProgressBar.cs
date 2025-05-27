using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigProgressBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(int max)
    {
        slider.maxValue = max;
        slider.value = 0;
    }

    public void UpdateBar(int current)
    {
        slider.value = current;
    }
}
