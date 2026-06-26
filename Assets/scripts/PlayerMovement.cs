using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
<<<<<<< Updated upstream
=======
    [SerializeField] private float health;

    [SerializeField] private bool canDouble = true;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    [SerializeField] private Vector3 spawnPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        Debug.Log(rb.velocity.y);
=======
    [SerializeField] private float fireRate = 0.2f;
    private float nextFireTime = 0f;
    private Vector3 spawnPos;

    [Header("dashing")]
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime = 0.5f;
    private bool canDash = true;
    private bool isDashing;

    int val = 0;
    public WheelScript wheel;
    [Header("Weapon")]
    private int currentWeapon = 0;
    private float gravityScale;
    private bool pendingShot = false;
    public int alginFlag = -1;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        gravityScale = rb.gravityScale;
    }

    private void SwitchWeapon(int weaponIndex)
    {
        int totalWeaponLayers = 4;
        for (int i = 1; i <= totalWeaponLayers; i++)
        {
            if (i == weaponIndex)
            {
                anim.SetLayerWeight(i, 1f);
            }
            else
            {
                anim.SetLayerWeight(i, 0f);
            }
        }
        anim.SetInteger("currentWeapon", weaponIndex);
        currentWeapon = weaponIndex;

        if (weaponIndex != 3)
        {
            alginFlag = -1;
        }
    }

    public void switchLayer(int weaponIndex)
    {
        SwitchWeapon(weaponIndex);
    }
    public int getFlag()
    {
        return alginFlag;
    }
    void Update()
    {
        if (wheel.activeWeapon == 3)
        {
            alginFlag = 3;
        }
        if (wheel.activeWeapon == 1)
        {
            alginFlag = 1;
        }
        if (wheel.activeWeapon == 2)
        {
            alginFlag = 2;
        }
        if (wheel.activeWeapon == 4)
        {
            alginFlag = 4;
        }
>>>>>>> Stashed changes
        handleCollision();
        handleInput();
        handleMovement();
        handleAnim();
<<<<<<< Updated upstream
        if (shot)
        {
            Instantiate(bulletObj , spawnPos, Quaternion.identity);
=======
        HandledDoubleJump();
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
                    canDouble = false;
                }
            }
>>>>>>> Stashed changes
        }
    }

    private void handleInput()
    {
<<<<<<< Updated upstream
=======
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            switchLayer(val);
            if (val == 0)
            {
                val = wheel.activeWeapon;

            }
            else
            {
                val = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartDash();
        }

>>>>>>> Stashed changes
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }
<<<<<<< Updated upstream

        if (Input.GetKey(KeyCode.Mouse0) && isGrounded)
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

=======
        if (Input.GetKey(KeyCode.Mouse0))
        {
            anim.SetTrigger("shot2");  // trigger instead of bool
            if (alginFlag == 1)
            {
                bulletObj.GetComponent<bulletScript>().bulletDir = dir ? 1 : -1;
                pendingShot = true;
            }
        }



    }



    private void StartDash()
    {
        rb.gravityScale = 0;
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
        rb.gravityScale = gravityScale;
        anim.SetBool("dash", false);
    }

>>>>>>> Stashed changes
    private void handleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, distanceToCheck, whatIsGround);
    }

    private void handleAnim()
    {
        anim.SetFloat("moveIdle", rb.velocity.x);
        anim.SetFloat("jumpFall", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
<<<<<<< Updated upstream
        anim.SetBool("shot", shot);
=======
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
}
=======
    public void EndAttack()
    {
        shot = false;
    }
    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            anim.SetBool("dead2", true);

        }
    }
    public void disableScript()
    {
        this.enabled = false;
    }
    public void destroyMe()
    {
        Destroy(gameObject);
    }
    public void fireUpBullut()
    {
        Debug.Log("fireUpBullut called | pendingShot: " + pendingShot + " | alginFlag: " + alginFlag);

        if (pendingShot && alginFlag == 1 && Time.time >= nextFireTime)
        {
            Debug.Log("BULLET SPAWNED");
            Instantiate(bulletObj, spawnPos, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
            pendingShot = false;  // consumed
        }
    }
}
>>>>>>> Stashed changes
