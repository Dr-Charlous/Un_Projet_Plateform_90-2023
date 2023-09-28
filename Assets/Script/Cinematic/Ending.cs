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
    public string _nameScene;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.GetComponent<PlayerController>()._activeOrNot = false;
            Rooms.SetActive(false);

            StartCoroutine(Transition.TimeAnimationEnd(_nameScene));
        }
    }
}
