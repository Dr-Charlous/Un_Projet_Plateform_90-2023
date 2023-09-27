using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition1 : MonoBehaviour
{
    public Button[] _buttons;
    public bool _animate;

    private void Start()
    {
        _animate = true;
    }

    private void SwitchButton()
    {
        _animate = !_animate;
    }
}
