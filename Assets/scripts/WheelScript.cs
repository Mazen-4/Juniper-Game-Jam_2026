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

    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isSpinning;
    float accum = 0f;
    float angularSpeed = 0f;
    [SerializeField] private Animator playerAnimator;

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
            rotate();
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
        if (rot > 0 + 45 && rot <= 90 + 45)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            playerAnimator.SetBool("canBlue",false);
            playerAnimator.SetBool("canPink", false);
            playerAnimator.SetBool("canGold", false);
            playerAnimator.SetBool("canRed", true);

        }
        else if (rot > 90 + 45 && rot <= 180 + 45)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            playerAnimator.SetBool("canBlue", false);
            playerAnimator.SetBool("canPink", false);
            playerAnimator.SetBool("canGold", true);
            playerAnimator.SetBool("canRed", false);
        }
        else if (rot > 180 + 45 && rot <= 270 + 45)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            playerAnimator.SetBool("canBlue", true);
            playerAnimator.SetBool("canPink", false);
            playerAnimator.SetBool("canGold", false);
            playerAnimator.SetBool("canRed", false);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            playerAnimator.SetBool("canBlue", false);
            playerAnimator.SetBool("canPink", true);
            playerAnimator.SetBool("canGold", false);
            playerAnimator.SetBool("canRed", false);
        }
    }

 
}