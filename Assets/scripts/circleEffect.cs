using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleEffect : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
           
            Animator anim = collision.GetComponentInChildren<Animator>();
            
            if (anim)
            {
                anim.SetTrigger("hit");
                soundManager.PlaySound(soundType.PLAYERHIT, 0.75f);


            }
            if (player)
            {
                Debug.Log("player hit");
                player.TakeDamage();
            }
           
        }
    }
}
