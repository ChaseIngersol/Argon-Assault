using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup Settings")] 
    [Tooltip("How fast the ship moves up and down based upon player input")] [SerializeField] float controlSpeed = 50f;

    [Header("Laser gun array")] 
    [Tooltip("Add all player lasers here:")]
    [SerializeField] GameObject[] lasers;

    [Header("Screen position based tuning")] 
    [Tooltip("Adjusts how much pitch is applied to ship based upon local y position")][SerializeField] float positionPitchFactor = 1.25f;
    [Tooltip("Adjusts how much yaw is applied to ship based upon local x position")][SerializeField] float positionYawFactor = -1.25f;

    [Header("Player input based tuning")] 
    [Tooltip("Adjusts how much pitch is applied to ship based upon up/down player input")][SerializeField] float controlPitchFactor = 8f;
    [Tooltip("Adjusts how much roll is applied to ship based upon left/right player input")][SerializeField] float controlRollFactor = 15f;

    float xThrow;
    float yThrow;

    // Update is called once per frame
    void Update()
	{
		ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
	}

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;

        float yawDueToPosition = transform.localPosition.x * positionYawFactor;

        float rollDueToControlThrow = xThrow * controlRollFactor;
        
        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = yawDueToPosition;
        float roll = rollDueToControlThrow;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

	void ProcessTranslation()
	{
		xThrow = Input.GetAxis("Horizontal");
		yThrow = Input.GetAxis("Vertical");

		float xOffset = -xThrow * controlSpeed * Time.deltaTime;
		float rawXPos = transform.localPosition.x + xOffset;
		float clampedXPos = Mathf.Clamp(rawXPos, -20f, 20f);

		float yOffset = yThrow * controlSpeed * Time.deltaTime;
		float rawYPos = transform.localPosition.y + yOffset;
		float clampedYPos = Mathf.Clamp(rawYPos, -10f, 17f);

		transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
	}
    void ProcessFiring()
    {
        //if pushing fire button
        if (Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

	void SetLasersActive(bool isActive)
	{
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
	}
}
