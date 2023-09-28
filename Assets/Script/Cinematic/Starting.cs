using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class Starting : MonoBehaviour
{
    public GameObject Player;
    public GameObject Doppleganger;
    public GameObject Rooms;
    public Transform _cam;
    public Vector3 _beginPos;
    public Vector3 _endPos;
    public float _speed;

    private void Start()
    {
        Player.SetActive(false);
        Doppleganger.SetActive(true);
        Rooms.SetActive(false);

        StartCoroutine(Wait());
        _cam.position = _beginPos;
    }

    private void Update()
    {
        if (Doppleganger.activeSelf)
        {
            
        }
        else
        {
            _cam.DOMove(_endPos, _speed);
        }
    }

    void SwitchPalyer()
    {
        Player.SetActive(!Player.activeSelf);
        Doppleganger.SetActive(!Doppleganger.activeSelf);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_speed * 10);
        Rooms.SetActive(true);
        SwitchPalyer();
    }
}
