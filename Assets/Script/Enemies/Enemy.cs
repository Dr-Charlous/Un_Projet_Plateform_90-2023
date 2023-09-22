using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Force = 10;
    public float Timer = 0.5f;

    float _timerValue;

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
        if (collision.GetComponent<PlayerController>() == null || _timerValue > 0)
            return;

        _timerValue = Timer;

        Vector2 direction = collision.transform.position - transform.position;
        direction.Normalize();

        var rb = collision.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * Force, ForceMode2D.Impulse);

    }


}
