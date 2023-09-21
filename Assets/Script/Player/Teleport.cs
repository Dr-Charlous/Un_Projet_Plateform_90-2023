using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Teleport : MonoBehaviour
{
    public GameObject Player;
    public GameObject TpPointPivot;
    public Collider2D CursorCollider;
    public float Range;
    public LayerMask Layer;

    private void Start()
    {
        DistanceTeleport();
    }

    private void Update()
    {
        Teleportation();
    }

    void DistanceTeleport()
    {
        //if (Range == TpPointPivot.transform.localPosition.x)
        //{
        //    return;
        //}
        //else
        //{
            transform.localPosition = Vector3.left * Range;
        //}
    }

    void Teleportation()
    {
        TpPointPivot.transform.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
        TpPointPivot.transform.Rotate(TpPointPivot.transform.rotation.x, TpPointPivot.transform.rotation.y, -90);

        if (Input.GetMouseButtonDown(0))
        {
            Player.transform.position = gameObject.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(Layer))
        {
            //TpPointPivot.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1f));
            Debug.Log("prout");
        }
    }
}
