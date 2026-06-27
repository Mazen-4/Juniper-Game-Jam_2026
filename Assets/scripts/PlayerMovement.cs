using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [Header("movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float health = 5f;

    // ── UI BARS: add maxHealth ──────────────────────────────
    [SerializeField] private float maxHealth = 5f;
    // ───────────────────────────────────────────────────────

    [SerializeField] private bool canDouble = true;
    [SerializeField] private bool dir;

    [SerializeField] private Animator anim;

    [Header("grounded check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float distanceToCheck;
    [SerializeField] bool isGrounded = true;

    [Header("shotting related")]
    [SerializeField] bool shot = false;
    [SerializeField] private GameObject bulletObj1;
    [SerializeField] private GameObject bulletObj2;

    [SerializeField] private float spawnDistance;
    [SerializeField] private float spawnDistanceUP;

    [SerializeField] private float fireRate = 0.2f;
    private float nextFireTime = 0f;
    private Vector3 spawnPos;

    [Header("dashing")]
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 2f;    // how long before you can dash again
    private bool canDash = true;
    private bool isDashing;

    // ── UI BARS: add nextDashTime ───────────────────────────
    private float nextDashTime = 0f;
    // ───────────────────────────────────────────────────────


    public int coinCount;
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

    // ── UI BARS: two getter methods ─────────────────────────
    public float GetHealthNormalized() => health / maxHealth;
    public float GetDashCooldownNormalized()
    {
        if (canDash) return 1f;
        return 1f - ((nextDashTime - Time.time) / dashCooldown);
    }
    // ───────────────────────────────────────────────────────
    private void SwitchWeapon(int weaponIndex)
    {
        int totalWeaponLayers = 4;
        for (int i = 1; i <= totalWeaponLayers; i++)
        {
            anim.SetLayerWeight(i, i == weaponIndex ? 1f : 0f);
        }
        anim.SetInteger("currentWeapon", weaponIndex);
        currentWeapon = weaponIndex;

        if (weaponIndex != 3)
        {
            alginFlag = -1;
        }

        wheel.SetActiveWeapon(weaponIndex);
    }

    public void switchLayer(int weaponIndex) { SwitchWeapon(weaponIndex); }
    public int getFlag() { return alginFlag; }

    void Update()
    {
       
            /*
            if (wheel.activeWeapon == 2 && Input.GetKeyDown(KeyCode.Mouse0)) soundManager.PlaySound(soundType.SWORD , 0.75f);

            if (wheel.activeWeapon == 1 && Input.GetKeyDown(KeyCode.Mouse0)) soundManager.PlaySound(soundType.GUN, 0.75f);


            if (wheel.activeWeapon == 3 && Input.GetKeyDown(KeyCode.Mouse0)) soundManager.PlaySound(soundType.MAGIC, 0.75f);


            if (wheel.activeWeapon == 4 && Input.GetKeyDown(KeyCode.Mouse0)) soundManager.PlaySound(soundType.AXE, 0.75f);
             */

            if (wheel.activeWeapon == 0 && Input.GetKeyDown(KeyCode.Mouse0)) soundManager.PlaySound(soundType.PANHIT, 0.5f);


        if (wheel.activeWeapon == 3) alginFlag = 3;
        if (wheel.activeWeapon == 1) alginFlag = 1;
        if (wheel.activeWeapon == 2) alginFlag = 2;
        if (wheel.activeWeapon == 4) alginFlag = 4;

        handleCollision();
        handleInput();
        handleMovement();
        handleAnim();
        HandledDoubleJump();
    }


    public void playSword()
    {
        soundManager.PlaySound(soundType.SWORD, 0.5f);
    }

    public void playGunForward()
    {
        soundManager.PlaySound(soundType.GUN, 0.5f);
        
    }

    public void playGunUp()
    {
        soundManager.PlaySound(soundType.MAGIC, 0.5f);
    }

    public void playAxe()
    {
        soundManager.PlaySound(soundType.AXE, 0.5f);
    }

    public void playPan()
    {
        soundManager.PlaySound(soundType.PANHIT, 0.5f);
    }


    public void playFootStep()
    {
        soundManager.PlaySound(soundType.FOOTSTEP, 0.5f);
    }
    public void playDash()
    {
        soundManager.PlaySound(soundType.PLAYERDASH, 0.5f);
    }
    public void playHit()
    {
        soundManager.PlaySound(soundType.PLAYERHIT, 0.5f);
    }
    void HandledDoubleJump()
    {
        if (!isGrounded)
        {

            if (canDouble && Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetTrigger("canDouble");
                canDouble = false;
            }
        }
        else
        {
            canDouble = true;
        }
    }

    private void handleInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            val = val == 0 ? wheel.lastSpunWeapon : 0;
            switchLayer(val);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartDash();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            soundManager.PlaySound(soundType.PLAYERJUMP, 0.75f);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            anim.SetTrigger("shot2");
            if (alginFlag == 1)
            {
                bulletObj1.GetComponent<bulletScript>().bulletDir = dir ? 1 : -1;
                pendingShot = true;
            }
            if (alginFlag == 3)
            {
                pendingShot = true;
            }
        }
    }
    public int getCurrentWeapon()
    {
        return currentWeapon;
    }
    


    private void StartDash()
    {
        rb.gravityScale = 0;
        float x = Input.GetAxisRaw("Horizontal");
        Vector2 dashDir = new Vector2(x, 0);

        if (dashDir == Vector2.zero)
            dashDir = new Vector2(dir ? 1 : -1, 0);

        isDashing = true;
        canDash = false;
        anim.SetBool("dash", true);
        rb.velocity = dashDir.normalized * dashPower;

        // ── UI BARS: record when dash ends ──────────────────
        nextDashTime = Time.time + dashCooldown;
        // ───────────────────────────────────────────────────

        StartCoroutine(StopDashing());
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);      // dash movement stops after dashTime
        isDashing = false;
        rb.gravityScale = gravityScale;
        anim.SetBool("dash", false);

        yield return new WaitForSeconds(dashCooldown - dashTime);  // wait remaining cooldown
        canDash = true;                                 // dash available again after full cooldown
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
    }

    public void Heal(float amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
    }
    private void handleMovement()
    {
        if (isDashing) return;

        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

        if (Input.GetAxisRaw("Horizontal") < 0 && dir) dir = false;
        if (Input.GetAxisRaw("Horizontal") > 0 && !dir) dir = true;

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

    public void EndAttack() { shot = false; }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
            anim.SetBool("dead2", true);
    }

    public void disableScript()
    {
        StartCoroutine(SlowMotionDeath());
    }

    private IEnumerator SlowMotionDeath()
    {
        soundManager.PlaySound(soundType.PLAYERDEATH);
        float duration = 0.8f;
        float targetScale = 0.05f;
        float startScale = Time.timeScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(startScale, targetScale, elapsed / duration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.3f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene("DeathScreen");
    }
    public void destroyMe() { Destroy(gameObject); }

    public void fireUpBullut()
    {

        if (pendingShot && Time.time >= nextFireTime)
        {
            if (alginFlag == 1)
            {
                GameObject b = Instantiate(bulletObj2, spawnPos, Quaternion.identity);
                bulletScript bs = b.GetComponent<bulletScript>();
                bs.bulletDir = dir ? 1 : -1;
                bs.bulletDirY = 0;
            }
            if (alginFlag == 3)
            {
                Vector3 upSpawnPos = transform.position + Vector3.up * spawnDistanceUP;
                GameObject b = Instantiate(bulletObj1, upSpawnPos, Quaternion.identity);
                bulletScript bs = b.GetComponent<bulletScript>();
                bs.bulletDir = 0;
                bs.bulletDirY = 1;
            }

            nextFireTime = Time.time + fireRate;
            pendingShot = false;
        }
    }
}


