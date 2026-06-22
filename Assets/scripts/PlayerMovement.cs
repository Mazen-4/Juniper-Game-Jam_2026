using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool canDouble=true;
    [SerializeField] private bool dir;
    

    [SerializeField] private Animator anim;

    [Header("grounded check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float distanceToCheck;
    [SerializeField] private bool isGrounded = true;

    [Header("shooting related")]
    [SerializeField] private bool shot = false;
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float fireRate = 0.2f;
    private float nextFireTime = 0f;
    private Vector3 spawnPos;

    [Header("dashing")]
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime = 0.5f;
    private bool canDash = true;
    private bool isDashing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (anim == null) { 
            anim = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        handleCollision();
        handleInput();
        handleMovement();
        handleAnim();
        handleShooting();
        HandledDoubleJump();
    }

    private bool HasActivePower()
    {
        return anim.GetBool("canPink") || anim.GetBool("canGold") || anim.GetBool("canRed") || anim.GetBool("canBlue");
    }

    void HandledDoubleJump()
    {
        if (!isGrounded)
        {
            if (canDouble)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    anim.SetTrigger("canDouble");
                    canDouble=false;
                }
            }
        }
    }
    private void handleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartDash();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            canDouble = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
         
        }

        bool powerActive = HasActivePower();

        if ((powerActive && Input.GetKey(KeyCode.Mouse0)) ||(!powerActive && Input.GetKeyDown(KeyCode.Mouse0)))
        {
            shot = true;
            bulletObj.GetComponent<bulletScript>().bulletDir = dir ? 1 : -1;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && powerActive)
        {
            shot = false;
        }
    }

    private void handleShooting()
    {
        if (shot && HasActivePower() && Time.time >= nextFireTime)
        {
            Instantiate(bulletObj, spawnPos, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }
    }

    private void StartDash()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Vector2 dashDir = new Vector2(x, 0);

        if (dashDir == Vector2.zero)
        {
            dashDir = new Vector2(x, 0);
        }

        isDashing = true;
        canDash = false;
        anim.SetBool("dash", true);

        rb.velocity = dashDir.normalized * dashPower;

        StartCoroutine(StopDashing());
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        anim.SetBool("dash", false);
    }

    private void handleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, distanceToCheck, whatIsGround);
        if (isGrounded)
        {
            canDash = true;
        }
    }

    private void handleAnim()
    {
        anim.SetFloat("moveIdle", rb.velocity.x);
        anim.SetFloat("jumpFall", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("shot", shot);
    }

    private void handleMovement()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

        if (Input.GetAxisRaw("Horizontal") < 0 && dir)
        {
            dir = false;
        }
        if (Input.GetAxisRaw("Horizontal") > 0 && !dir)
        {
            dir = true;
        }

        if (dir)
        {
            spawnPos = transform.position + Vector3.right * spawnDistance;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            spawnPos = transform.position + Vector3.left * spawnDistance;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - distanceToCheck));
    }

    public void EndAttack()
    {
        shot = false;
    }
}