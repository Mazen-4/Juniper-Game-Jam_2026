using System.Collections;
using UnityEngine;

public class Spikes_obstacle : MonoBehaviour
{
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private float knockbackForceY = 7f;

    private float _damageTimer;

    private void Update()
    {
        if (_damageTimer > 0f)
            _damageTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) => HitPlayer(other);
    private void OnTriggerStay2D(Collider2D other) => HitPlayer(other);

    private void HitPlayer(Collider2D other)
    {
        if (!other.CompareTag("Player") || _damageTimer > 0f) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player)
        {
            _damageTimer = damageInterval;
            player.TakeDamage(0.5f);
            player.ApplyKnockback(knockbackForceY);
        }
    }
}