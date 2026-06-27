using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WheelScript : MonoBehaviour
{
    public float rotatePowerS;
    public float rotatePowerE;

    public float stopPowerS;
    public float stopPowerE;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isSpinning;
    float accum = 0f;
    float angularSpeed = 0f;
    [SerializeField] private Animator playerAnimator;
    public int activeWeapon;
    public int lastSpunWeapon = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void rotate()
    {
        if (!isSpinning)
        {
            float rotatePower = Random.Range(rotatePowerS, rotatePowerE);
            angularSpeed = rotatePower;
            isSpinning = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CoinManager.getCoinCt() > 5)
            {
                rotate();
                CoinManager.setCoinCt(CoinManager.getCoinCt() - 5);  
            }

        }
        if (angularSpeed > 0)
        {
            float stopPower = Random.Range(stopPowerS, stopPowerE);

            angularSpeed -= stopPower * Time.deltaTime;
            angularSpeed = Mathf.Clamp(angularSpeed, 0, 1440);

            transform.Rotate(0, 0, angularSpeed * Time.deltaTime);
        }

        if (angularSpeed <= 0 && isSpinning)
        {
            accum += 1 * Time.deltaTime;
            if (accum >= 0.5f)
            {
                GetReward();
                isSpinning = false; 
                accum = 0;
            }
        }
    }
    
    void GetReward()
    {
        float rot = transform.eulerAngles.z;
        if (rot > 45 && rot <= 135)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            player.switchLayer(1); // gun1
           
            activeWeapon = 1;
            lastSpunWeapon = 1;
            Debug.Log("weapon1");
        }
        else if (rot > 135 && rot <= 225)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            player.switchLayer(2); // gun2
            activeWeapon = 2;
            lastSpunWeapon = 2;

            Debug.Log("weapon2");


        }
        else if (rot > 225 && rot <= 315)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            player.switchLayer(3); // gun3
            activeWeapon = 3;
            lastSpunWeapon = 3;

            Debug.Log("weapon3");

        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            player.switchLayer(4);
            activeWeapon = 4;
            lastSpunWeapon = 4;

            Debug.Log("weapon4");

        }
    }
    public void SetActiveWeapon(int index)
    {
        activeWeapon = index;
    }
}