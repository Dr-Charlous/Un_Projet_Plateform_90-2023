using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Teleport : MonoBehaviour
{
    public GameObject Player;
    public GameObject TpPointPivot;
    public GameObject TpPoint;
    public Collider2D CursorCollider;
    public float Range;
    public float CooldownTeleport;
    public float TimeSinceTeleport;
    public bool CanTeleport;
    public LayerMask Layer;
    public Sound _teleportSound;
    public AudioSource _audioSource;
    public Animator _animatorPlayer;

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

        TpPoint.transform.rotation = Player.transform.rotation;
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

        if (TimeSinceTeleport > CooldownTeleport && Player.GetComponent<PlayerController>().NumberTeleport > 0)
        {
            _animatorPlayer.SetBool("Hat", true);
        }

        if (Player.GetComponent<PlayerController>().teleportationClick && TimeSinceTeleport > CooldownTeleport && CanTeleport && Player.GetComponent<PlayerController>().NumberTeleport > 0)
        {
            Player.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 0.3f, gameObject.transform.position.z);
            TimeSinceTeleport = 0;
            Player.GetComponent<PlayerController>().NumberTeleport -= 1;
            Player.GetComponent<PlayerController>().teleportationClick = false;

            Player.GetComponent<PlayerController>().PlaySound(_teleportSound, _audioSource);

            _animatorPlayer.SetBool("Hat", false);
            _animatorPlayer.SetTrigger("TP");
        }
    }
}
