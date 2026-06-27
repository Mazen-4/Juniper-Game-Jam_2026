using System.Collections;
using UnityEngine;

public class Spikes_obstacle : MonoBehaviour
{
    public float damageCooldown = 1f;
    private float cooldownTimer = 0f;

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HitPlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        HitPlayer(other);
    }

    private void HitPlayer(Collider2D other)
    {
        if (cooldownTimer > 0f) return;

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player)
            {
                Animator anim = other.gameObject.GetComponentInChildren<Animator>();
                anim.SetTrigger("hit");
                player.TakeDamage();
                player.rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
                cooldownTimer = damageCooldown;
            }
        }
    }
}