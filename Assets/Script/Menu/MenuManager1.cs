using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UIElements;

public class MenuManager1 : MonoBehaviour
{
    [Header("Cam move")]
    public Camera _cam;
    public float _camSpeed;
    public Vector3 _camBeginPos;
    public Vector3 _camEndPos;

    private void Start()
    {
        _cam.transform.position = new Vector3(_camBeginPos.x, 0, _camBeginPos.z);
    }

    public void Update()
    {
        CamMove();
    }

    void CamMove()
    {
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camEndPos, _camSpeed * Time.deltaTime);

        if (_cam.transform.position.y >= _camEndPos.y -2)
        {
            _cam.transform.position = _camBeginPos;
        }
    }
}
