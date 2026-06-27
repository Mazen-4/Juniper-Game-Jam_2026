using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public class osamaScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float lowerBound;
    [SerializeField] private float upperBound;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;



    [Header("mercyFactor")]
    [SerializeField] private float minReactionTime;
    [SerializeField] private float maxReactionTime;
    [SerializeField] private float reactionTimer;
    [SerializeField] private bool waiting;
    [SerializeField] private float chargePower;
    [SerializeField] private float ogHeight;
    [SerializeField] private float returnSpeed;
    [SerializeField] private bool attack;

    [Header("petroling")]
    [SerializeField] private bool isPetrol;
    [SerializeField] private float dangerZone;
    [SerializeField] private float minPatrol;
    [SerializeField] private float maxPatrol;
    [SerializeField] private bool movingRight = true;
    private float leftPoint;
    private float rightPoint;
    [SerializeField] private float patrolSpeed;


    [SerializeField] private float health;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ogHeight = transform.position.y;

        leftPoint = transform.position.x - minPatrol;
        rightPoint = transform.position.x + maxPatrol;
    }

    void Update()
    {
        if (anim.GetBool("charge"))
        {
            attack = true;
            reactionTimer = 0;
        }
        if (attack)
        {
            rb.AddForce(Vector2.down * chargePower * Time.deltaTime, ForceMode2D.Impulse);

        }
        if (anim.GetBool("hitSomething"))
        {
            // get back to og height slowly

            Vector3 pos = transform.position;

            pos.y = Mathf.MoveTowards(pos.y, ogHeight, returnSpeed * Time.deltaTime);

            transform.position = pos;

            if (Mathf.Abs(pos.y - ogHeight) < 0.05f)
            {
                anim.SetBool("hitSomething", false);
            }
        }

        if (isPetrol)
        {
            if (movingRight)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);

                rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);

                if (transform.position.x >= rightPoint)
                    movingRight = false;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);

                rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);

                if (transform.position.x <= leftPoint)
                    movingRight = true;
            }
        }
    }
    private void FixedUpdate()
    {

        float distanceX = player.position.x - transform.position.x;
        bool isPlayerOutside = distanceX > upperBound || distanceX < lowerBound;
        bool isInDangerZone = math.abs(distanceX) < dangerZone;
        if (isInDangerZone)
        {
            isPetrol = false;
            if (isPlayerOutside)
            {
                if (!waiting)
                {
                    reactionTimer = Random.Range(minReactionTime, maxReactionTime);
                    waiting = true;
                }
                reactionTimer -= Time.deltaTime;
                if (reactionTimer > 0)
                {
                    rb.velocity = Vector2.zero;
                    return;
                }

                if (distanceX > upperBound)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
                anim.SetBool("playerUnder", false);
                anim.SetBool("charge", false);

            }
            else
            {
                waiting = false;
                anim.SetBool("charge", true);
                anim.SetBool("playerUnder", true);
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

        }

        else
        {
            isPetrol = true;

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("hitSomething", true);
        attack = false;
    }

    public void takeDamage(int damage)
    {
        soundManager.PlaySound(soundType.OSAMAHIT);

        health -= damage;


        if (health <= 0)
        {
            anim.SetTrigger("dead");
        }
    }

    public void destroyMe()
    {
        Destroy(gameObject);
    }
}
