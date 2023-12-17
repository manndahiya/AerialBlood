using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] InputAction movement;


    // private void OnEnable()
    // {
    //    movement.Enable();
    //  }

    //  private void OnDisable()
    //  {
    //      movement.Disable(); 
    // }
   [SerializeField] float XmoveSpeed = 10f;
   [SerializeField] float YmoveSpeed = 20f;
   [SerializeField] float xRange = 10f;
   [SerializeField] float yRange = 10f;

    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float positionYawFactor = 5f;
    [SerializeField] float positionRollFactor = 5f;
    [SerializeField] float controlRollFactor = 15f;

    float yThrow, xThrow;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();

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
}
