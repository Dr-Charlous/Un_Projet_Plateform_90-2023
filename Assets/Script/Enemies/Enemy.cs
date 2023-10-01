using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Force = 10;
    public float Timer = 0.5f;

    float _timerValue;

    public AudioSource _audioSource1;
    public AudioSource _audioSource2;
    public AudioClip _outSound;
    public AudioClip _punchSound;
    public Animator _animator;

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

        _audioSource1.clip = _outSound;
        _audioSource1.Play();

        _timerValue = Timer;


        Vector2 direction = collision.transform.position - transform.position;
        direction.Normalize();

        var rb = collision.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * Force, ForceMode2D.Impulse);

        _audioSource2.clip = _punchSound;
        _audioSource2.Play();
    }
}
