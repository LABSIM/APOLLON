using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadiosondeFrontend
    : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(int max_value)
    {

        slider.maxValue = max_value; // max value
        slider.value = 20; // start value

    } /**/

    public void SetValue(float value)
    {
        slider.value = value;
    }

}
