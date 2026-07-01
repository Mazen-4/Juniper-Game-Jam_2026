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
    [SerializeField] private int coinsToDrop = 3;
    public int activeWeapon;
    public int lastSpunWeapon = 0;

    private float spinDuration = 3f;
    private float soundClipLength = 3f;
        private AudioSource spinAudioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spinAudioSource = gameObject.AddComponent<AudioSource>();
        spinAudioSource.clip = soundManager.GetClip(soundType.SPIN);
        spinAudioSource.loop = false;
        spinAudioSource.playOnAwake = false;
    }
    float EstimateSpinDuration(float startSpeed)
    {
        float speed = startSpeed;
        float time = 0f;
        while (speed > 0)
        {
            float stopPower = (stopPowerS + stopPowerE) / 2f;
            speed -= stopPower * 0.016f;
            speed = Mathf.Clamp(speed, 0, 1440);
            time += 0.016f;
            if (time > 30f) break;
        }
        return time + 0.5f;
    }
    public void rotate()
    {
        if (!isSpinning)
        {
            float rotatePower = Random.Range(rotatePowerS, rotatePowerE);
            angularSpeed = rotatePower;
            isSpinning = true;

            float spinDuration = EstimateSpinDuration(angularSpeed);
            spinAudioSource.pitch = soundClipLength / spinDuration;
            spinAudioSource.Play();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CoinManager.getCoinCt() > coinsToDrop)
            {
                rotate();
                CoinManager.setCoinCt(CoinManager.getCoinCt() - coinsToDrop);  
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
            if (spinAudioSource.isPlaying)
                spinAudioSource.Stop();

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