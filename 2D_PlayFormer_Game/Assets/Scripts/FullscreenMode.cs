using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenMode : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
        toggle.onValueChanged.AddListener(delegate
        {
            Screen.fullScreen = !Screen.fullScreen;
        });
    }
}
