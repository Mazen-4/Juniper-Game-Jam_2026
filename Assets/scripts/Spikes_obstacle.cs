using System.Collections;
using UnityEngine;

public class Spikes_obstacle : MonoBehaviour
{
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private PlayerMovement player;  // drag player here in Inspector
    private bool isDamaging = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player.gameObject && !isDamaging)
            StartCoroutine(DamageLoop());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player.gameObject)
            isDamaging = false;
    }

    private IEnumerator DamageLoop()
    {
        isDamaging = true;
        while (isDamaging)
        {
            player.TakeDamage();
            yield return new WaitForSeconds(damageInterval);
        }
    }
}