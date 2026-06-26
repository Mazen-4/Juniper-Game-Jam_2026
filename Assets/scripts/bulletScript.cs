using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField]private int bulletSpeed;
    [SerializeField] public int bulletDir;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * bulletDir * Time.deltaTime * bulletSpeed;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            osamaScript osama = collision.gameObject.GetComponent<osamaScript>();
            ramadanScript ramadan = collision.gameObject.GetComponent<ramadanScript>();

            if (osama)
            {
                osama.takeDamage(1);
                Animator osamaAnim = osama.GetComponentInChildren<Animator>();
                osamaAnim.SetTrigger("hit");
                Debug.Log("bullet hit osama");

            }
            if (ramadan)
            {
                Debug.Log("bullet hit ramadan");
                Animator ramadanAnim = ramadan.GetComponentInChildren<Animator>();
                ramadanAnim.SetTrigger("hit");
                ramadan.TakeDamage(1);


            }
            Destroy(gameObject);
        }
    }

}
