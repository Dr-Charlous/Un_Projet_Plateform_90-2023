using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Camera.position = new Vector3(Camera.position.x, Camera.position.y + Range, Camera.position.z);
        }
        else if(Down.GetComponent<Collider2D>().IsTouching(Player))
        {
            Camera.position = new Vector3(Camera.position.x, Camera.position.y - Range, Camera.position.z);
        }
    }
}
