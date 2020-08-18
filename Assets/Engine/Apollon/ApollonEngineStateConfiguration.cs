using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApollonEngineStateConfiguration : MonoBehaviour
{
    void Awake()
    {

        // application wide settings goes here by default :)
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture
            = System.Globalization.CultureInfo.DefaultThreadCurrentUICulture
            = System.Globalization.CultureInfo.InvariantCulture;

    }

}
