using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraC : MonoBehaviour
{
    public Collider2D Up;
    public Collider2D Down;
    public Collider2D Player;
    public Transform Camera;
    public float Range;

    private void Update()
    {
        if (Up.GetComponent<Collider2D>().IsTouching(Player))
        {
            //Camera.position = new Vector3(Camera.position.x, Camera.position.y + Range, Camera.position.z);
            Camera.DOMove(new Vector3(Camera.position.x, Camera.position.y + Range, Camera.position.z), 2, true);
        }
        
        if(Down.GetComponent<Collider2D>().IsTouching(Player))
        {
            //Camera.position = new Vector3(Camera.position.x, Camera.position.y - Range, Camera.position.z);
            Camera.DOMove(new Vector3(Camera.position.x, Camera.position.y + Range, Camera.position.z), 2, true);
        }
    }
}
