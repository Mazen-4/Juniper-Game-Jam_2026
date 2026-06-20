using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool dir;


    [SerializeField] private Animator anim;

    [Header("grounded check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float distanceToCheck;
    [SerializeField] bool isGrounded = true;


    [Header("shotting related")]
    [SerializeField] bool shot = false;
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private float spawnDistance;
    [SerializeField] private Vector3 spawnPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        Debug.Log(rb.velocity.y);
        handleCollision();
        handleInput();
        handleMovement();
        handleAnim();
        if (shot && (anim.GetBool("canPink")|| anim.GetBool("canGold") || anim.GetBool("canRed")|| anim.GetBool("canBlue")) )
        {
            Instantiate(bulletObj , spawnPos, Quaternion.identity);
        }
    }

    private void handleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }

        if (Input.GetKey(KeyCode.Mouse0) )
        {
            shot = true;
            if (dir)
            {
                bulletObj.GetComponent<bulletScript>().bulletDir = 1;
            }
            else
            {
                bulletObj.GetComponent<bulletScript>().bulletDir = -1;

            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shot = false;
        }

       
    }

    private void handleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, distanceToCheck, whatIsGround);
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

}
