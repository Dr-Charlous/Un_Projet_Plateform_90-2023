using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Starting : MonoBehaviour
{
    public PlayerController Player;
    public Ending _end;
    public GameObject Rooms;
    public Transform _cam;
    public Vector3 _beginPos;
    public Vector3 _endPos;
    public float _speed;
    public float _timeToMove;
    public Animator _animator;
    public GameObject TextBegin;

    private void Start()
    {
        Player._activeOrNot = false;
        Rooms.SetActive(false);

        StartCoroutine(Wait());
        _cam.position = _beginPos;
    }

    [System.Obsolete]
    private void Update()
    {
        if (Player._activeOrNot)
        {
            return;
        }
        else
        {
            _cam.DOMove(_endPos, _speed * 100 * Time.deltaTime);
        }

    }

    void SwitchPalyer()
    {
        Player._activeOrNot = !Player._activeOrNot;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_timeToMove);
        Rooms.SetActive(true);
        SwitchPalyer();
        _end._active = true;
        TextBegin.SetActive(true);
        yield return new WaitForSeconds(5);
        _animator.SetTrigger("End");
        yield return new WaitForSeconds(2);
        TextBegin.SetActive(false);
        Destroy(this);
    }
}
