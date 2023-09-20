using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject Cursor;
    public Collider2D CursorCollider;
    public float Range;
    public LayerMask Ground;

    private void Update()
    {
        Teleportation();
    }

    void Teleportation()
    {
        Cursor.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform == Cursor.transform)
        {
            //Cursor.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1f));
            Debug.Log("prout");
        }
    }
}
