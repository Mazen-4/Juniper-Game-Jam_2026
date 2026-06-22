using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ramadanScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;

    [Header("Patrolling")]
    [SerializeField] private float minPatrol = 3f;
    [SerializeField] private float maxPatrol = 6f;
    [SerializeField] private float patrolSpeed = 2f;
    private bool movingRight = true;
    private float leftPoint;
    private float rightPoint;
    private bool isPetrol;

    [Header("Danger Zone")]
    [SerializeField] private float dangerZone = 5f;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float lowerBound = -1f;
    [SerializeField] private float upperBound = 1f;

    [Header("Mercy Factor")]
    [SerializeField] private float minReactionTime = 0.5f;
    [SerializeField] private float maxReactionTime = 1.5f;
    private float reactionTimer;
    private bool waiting;


    [SerializeField] private bool attack =false;

    [SerializeField] private float health = 3;

    [SerializeField] private bool canAttack = true;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        leftPoint = transform.position.x - minPatrol;
        rightPoint = transform.position.x + maxPatrol;

        isPetrol = true;
    }

    private void Update()
    {
        if (isPetrol)
        {
            Patrol();
        }
        
    }

    private void FixedUpdate()
    {
        if (anim.GetBool("playerInRange") && canAttack)
        {
            canAttack = false;
            anim.SetInteger("index", Random.Range(0, 3));
            anim.SetTrigger("attack");
        }
        float distanceX = player.position.x - transform.position.x;
        
        bool isPlayerOutside = math.abs(distanceX) > upperBound;
        bool isInDangerZone = math.abs(distanceX) < dangerZone;
      
        if (isInDangerZone)
        {
            isPetrol = false;

            if (isPlayerOutside)
            {
                canAttack = true;
                if (!waiting)
                {
                    reactionTimer = Random.Range(minReactionTime, maxReactionTime);
                    waiting = true;
                }

                reactionTimer -= Time.fixedDeltaTime;

                if (reactionTimer > 0)
                {
                    rb.velocity = Vector2.zero;
                    return;
                }

                if (distanceX > upperBound)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }

                anim.SetBool("playerInRange", false);
                anim.SetBool("move", true);

            }
            else
            {
                waiting = false;
               
                anim.SetBool("playerInRange", true);


            }
        }
        else
        {
            isPetrol = true;
        }
    }

    private void Patrol()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(0, 0, 0);

            if (transform.position.x >= rightPoint)
                movingRight = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);

            if (transform.position.x <= leftPoint)
                movingRight = true;
        }
        anim.SetBool("playerInRange", false);

        anim.SetBool("move", true);
    }

    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            anim.SetTrigger("dead");
        }
    }

    public void destroyMe()
    {
        Destroy(gameObject);
    }
    public void EndAttack()
    {
        StartCoroutine(AttackCooldown());
    }
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1f); 
        canAttack = true;
    }
}