using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class Ending : MonoBehaviour
{
    public Transition Transition;
    public GameObject Rooms;
    public GameObject Cursor;
    public Animator _animatorText;
    public string _nameScene;
    public float _timer;
    public TextMeshProUGUI textMeshProUGUI;
    public float _score;
    public bool _active;

    private void Update()
    {
        if (_active)
        {
            _score += Time.deltaTime;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            _active = false;
            textMeshProUGUI.text = $@"You took so much time that the mad sorcerer died before you come...
You took 3 years 2 months 15 days {(int)_score/60} min and {(int)_score} sec";
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
