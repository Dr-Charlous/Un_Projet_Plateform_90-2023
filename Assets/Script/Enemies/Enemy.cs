using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Force = 10;
    public float Timer = 0.5f;

    private float _timerValue;

    public AudioSource _audioSource;
    public AudioClip _punchSound;
    public Animator _animator;
    private Collider2D _collision;

    private void Awake()
    {
        _timerValue = Timer;
    }

    private void Update()
    {
        _timerValue -= Time.deltaTime;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() == null || _timerValue > 0 || collision.transform.position.y < transform.position.y)
            return;

        if (collision.GetComponent<PlayerController>() != null && _timerValue < 0)
        {
            _animator.SetTrigger("Hit");
        }

        _collision = collision;
        StartCoroutine(WaitPuch());
    }

    IEnumerator WaitPuch()
    {
        yield return new WaitForSeconds(0.1f);

        _timerValue = Timer;


        Vector2 direction = _collision.transform.position - transform.position;
        direction.Normalize();

        var rb = _collision.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * Force, ForceMode2D.Impulse);

        _audioSource.clip = _punchSound;
        _audioSource.Play();
    }
}
