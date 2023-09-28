using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class Starting : MonoBehaviour
{
    public PlayerController Player;
    public GameObject Doppleganger;
    public GameObject Rooms;
    public Transform _cam;
    public Vector3 _beginPos;
    public Vector3 _endPos;
    public float _speed;
    public float _timeToMove;

    private void Start()
    {
        Player._activeOrNot = false;
        Doppleganger.SetActive(true);
        Rooms.SetActive(false);

        StartCoroutine(Wait());
        _cam.position = _beginPos;
    }

    [System.Obsolete]
    private void Update()
    {
        _cam.DOMove(_endPos, _speed);

        if (Player._activeOrNot)
        {
            return;
        }

        _cam.DOMove(_endPos, _speed * 100 * Time.deltaTime);
    }

    void SwitchPalyer()
    {
        Player._activeOrNot = !Player._activeOrNot;
        Doppleganger.SetActive(!Doppleganger.activeSelf);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_timeToMove);
        Rooms.SetActive(true);
        SwitchPalyer();
        yield return new WaitForSeconds(3);
        Destroy(this);
    }
}
