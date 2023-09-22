using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public GameObject Camera;
    public Collider2D PlayerCollision;
    public Vector3 PreviusPosition;
    public Vector3 NextPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == PlayerCollision)
        {
            if (PlayerCollision.transform.position.y < transform.position.y)
            {
                Camera.transform.DOMove(NextPosition, 2, false);
            }
            else
            {
                Camera.transform.DOMove(PreviusPosition, 2, false);
            }
        }
    }
}
