using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public Animator _animator;
    public bool _animate;
    public bool _canChangeScene;
    public float _fadeTime;
    public MenuManager _menuManager;

    private void Start()
    {
        _animate = true;
        _canChangeScene = false;
        _animator.SetTrigger("Idle");

        StartCoroutine(TimeAnimationStart());
    }

    private void SwitchButton(bool _active)
    {
        _animate = _active;
    }

    private IEnumerator TimeAnimationStart()
    {
        yield return new WaitForSeconds(_fadeTime);
        SwitchButton(true);
    }

    public IEnumerator TimeAnimationEnd(bool PlayOrQuit)
    {
        SwitchButton(false);
        _animator.SetTrigger("End");

        yield return new WaitForSeconds(_fadeTime);
        _canChangeScene = true;

        if (_menuManager != null)
        {
            if (PlayOrQuit)
            {
                _menuManager.PlayButton(_menuManager._nameSceneTraget);
            }
            else
            {
                _menuManager.QuitButton();
            }
        }
    }
}
