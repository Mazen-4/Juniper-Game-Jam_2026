using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.gameObject.CompareTag("Player"))
        {
            collected = true;
            CoinManager.instance.AddCoin();
            soundManager.PlaySound(soundType.COIN);
            Destroy(transform.parent.gameObject);
        }
    }
}