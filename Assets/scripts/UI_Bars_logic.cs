using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Bars_logic : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(-5f, 7f, 0f);

    [Header("UI Bars")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;

    private PlayerMovement playerMovement;
    
    private void Start()
    {
        playerMovement = target.GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        // Follow player
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Update bars
        if (playerMovement != null)
        {
            healthSlider.value = playerMovement.GetHealthNormalized();
            staminaSlider.value = playerMovement.GetDashCooldownNormalized();
        }
    }
}
