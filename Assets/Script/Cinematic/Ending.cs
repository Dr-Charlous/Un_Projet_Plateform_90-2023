using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public Transition Transition;
    public GameObject Rooms;
    public GameObject Cursor;
    public Animator _animatorText;
    public string _nameScene;
    public float _timer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.GetComponent<PlayerController>()._activeOrNot = false;
            Rooms.SetActive(false);
            Cursor.SetActive(false);

            _animatorText.SetTrigger("Text");
            StartCoroutine(WaitEnd());
        }

        IEnumerator WaitEnd()
        {
            yield return new WaitForSeconds(_timer);
            StartCoroutine(Transition.TimeAnimationEnd(_nameScene));
        }
    }
}
