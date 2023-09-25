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
    public float CooldownTeleport;
    public float TimeSinceTeleport;
    public bool CanTeleport;
    public LayerMask Layer;

    private void Start()
    {
        transform.localPosition = Vector3.left * Range;

        TimeSinceTeleport = CooldownTeleport;
    }

    private void Update()
    {
        CanTeleportHere();
        CursorTeleport();
        Teleportation();
    }

    public void CanTeleportHere()
    {
        if (CursorCollider.IsTouchingLayers(Layer))
        {
            CanTeleport = false;
            return;
        }
        else
        {
            CanTeleport = true;
        }
    }

    public void CursorTeleport()
    {
        Vector3 CursorInput = Vector3.zero;

        if (Player.GetComponent<PlayerController>().IsGamepad) 
        {
            CursorInput = Player.transform.position + (Vector3)Player.GetComponent<PlayerController>()._cursor;
        }
        else
        {
            CursorInput = (Vector3)Player.GetComponent<PlayerController>()._cursor;
        }


        //LookAt
        Vector3 diff = CursorInput - TpPointPivot.transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        TpPointPivot.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 180);
    }

    public void Teleportation()
    {
        TimeSinceTeleport += Time.deltaTime;

        if (Player.GetComponent<PlayerController>().teleportationClick && TimeSinceTeleport > CooldownTeleport && CanTeleport && Player.GetComponent<PlayerController>().NumberTeleport > 0)
        {
            Player.transform.position = gameObject.transform.position;
            TimeSinceTeleport = 0;
            Player.GetComponent<PlayerController>().NumberTeleport -= 1;
            Player.GetComponent<PlayerController>().teleportationClick = false;
        }
    }
}
