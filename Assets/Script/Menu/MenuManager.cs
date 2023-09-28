using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Cam move")]
    public Camera _cam;
    public float _camSpeed;
    public Vector3 _camBeginPos;
    public Vector3 _camEndPos;
    public string _nameSceneTraget;
    public Transition _transition;

    private void Start()
    {
        _cam.transform.position = _camBeginPos;
    }

    public void Update()
    {
        CamMove();
    }

    void CamMove()
    {
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camEndPos, _camSpeed * Time.deltaTime);

        if (_cam.transform.position.y >= _camEndPos.y - 2)
        {
            _cam.transform.position = _camBeginPos;
        }
    }

    public void PlayButton(string nameScene)
    {
        _nameSceneTraget = nameScene;
        StartCoroutine(_transition.TimeAnimationEnd(true));

        if (_transition._animate)
            return;

        if (_transition._canChangeScene)
        {
            SceneManager.LoadScene(nameScene);
        }
    }

    public void QuitButton()
    {
        StartCoroutine(_transition.TimeAnimationEnd(false));


        if (_transition._animate)
            return;

        if (_transition._canChangeScene)
        {
            Application.Quit();
        }
    }
}
