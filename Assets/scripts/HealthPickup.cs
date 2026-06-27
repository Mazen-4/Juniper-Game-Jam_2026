using UnityEngine;
public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement pm = other.GetComponentInParent<PlayerMovement>();
            if (pm == null)
                pm = other.GetComponent<PlayerMovement>();

            if (pm != null)
            {
                soundManager.PlaySound(soundType.HEAL);
                pm.Heal(healAmount);
                Debug.Log("potion healed player");
                Destroy(transform.parent.gameObject); 
            }
            else
            {
                Debug.Log("no PlayerMovement found");
            }
        }
    }
}