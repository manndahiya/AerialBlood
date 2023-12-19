using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction fire;


    [Header("General Setup Settings")]
    [Tooltip("How Fast ship moves")]
   [SerializeField] float XmoveSpeed = 10f;
   [SerializeField] float YmoveSpeed = 20f;
   [SerializeField] float xRange = 10f;
   [SerializeField] float yRange = 10f;
   [SerializeField] GameObject[] lasers;

    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 5f;
    [SerializeField] float positionRollFactor = 5f;

    [Header("Player Input based tuning")]
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float controlRollFactor = 15f;

    
    float yThrow, xThrow;

    


    private void OnEnable()
    {
        fire.Enable();
    }

    private void OnDisable()
    {
        fire.Disable();
    }

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();

    }

    void ProcessRotation()
    {
        //pitch
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        //yaw
        float yawDueToPosition = transform.localPosition.x * positionYawFactor;
        float yaw = yawDueToPosition + xThrow;

        //roll
        float rollDueToControlThrow = xThrow * controlRollFactor;
        float roll = transform.localPosition.z  + rollDueToControlThrow;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
    private void ProcessTranslation()
    {
         xThrow = Input.GetAxis("Horizontal");
         yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * XmoveSpeed * Time.deltaTime;
        float RawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(RawXPos, -xRange, xRange);

        float yOffset = yThrow * YmoveSpeed * Time.deltaTime;
        float RawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(RawYPos, -yRange, yRange);

        transform.localPosition = new Vector3
            (clampedXPos, clampedYPos, transform.localPosition.z);
    }


    void ProcessFiring()
    {
        if(fire.ReadValue<float>()>0.5)
        {
            SetLaserActive(true);
        }

        else
        {
            SetLaserActive(false);
        }
    }

    private void SetLaserActive(bool isActive)
    {

       
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }

    }
}
