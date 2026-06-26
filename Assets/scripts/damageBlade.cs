using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageBlade : MonoBehaviour
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

        PlayerMovement player = GetComponentInParent<PlayerMovement>();
        int currWeapon = player.getCurrentWeapon();

      
        if (collision.CompareTag("enemy"))
        {
            int dam = 1;
            if (currWeapon == 1)
            {
                dam = 1;
            }
            if (currWeapon == 2)
            {
                dam = 3;
            }
            if (currWeapon == 3)
            {
                dam = 2;
            }
            if (currWeapon == 4)
            {
                dam = 5;
            }
            else
            {
                dam = 1;
            }
                osamaScript osama = collision.GetComponent<osamaScript>();
            ramadanScript ramadan = collision.GetComponent<ramadanScript>();
            Animator anim= collision.GetComponentInChildren<Animator>();
            if (anim)
            {
                anim.SetTrigger("hit");
               
            }
            if (osama)
            {
                osama.takeDamage(dam);
            }
            if (ramadan)
            {
                ramadan.TakeDamage(dam);
            }
        }
    }
}
