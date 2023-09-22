using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Room : MonoBehaviour
{
    public GameObject Camera;
    public Collider2D PlayerCollision;
    public float TimeChangeScene = 1;
    public bool Snapping = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == PlayerCollision)
        {
            Camera.transform.DOMove(new Vector3(transform.position.x, transform.position.y, Camera.transform.position.z), TimeChangeScene, Snapping);
        }
    }
}
