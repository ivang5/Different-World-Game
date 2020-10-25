using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    [Header("General")]
    [Tooltip("In ms^-1")] [SerializeField] float controlSpeed = 8f;
    [Tooltip("In m")] [SerializeField] float xRange = 6f;
    [Tooltip("In m")] [SerializeField] float yRange = 3f;
    [SerializeField] GameObject[] guns;

    [Header("Screen-position Based")]
    [SerializeField] float positionPithcFactor = -3f;
    [SerializeField] float controlPitchFactor = -15f;

    [Header("Control-throw Based")]
    [SerializeField] float positionYawFactor = 5f;
    [SerializeField] float controlRollFactor = -20f;

    float xThrow, yThrow;
    bool isControlEnabled = true;

    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            Movement();
            Rotation();
            Firing();
        }
    }

    void OnPlayerDeath() // Called by String reference
    {
        isControlEnabled = false;
    }

    private void Movement()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * controlSpeed * Time.deltaTime;
        float yOffset = yThrow * controlSpeed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float yRawPos = transform.localPosition.y + yOffset;

        float xLimiter = Mathf.Clamp(rawXPos, -xRange, xRange);
        float yLimiter = Mathf.Clamp(yRawPos, -yRange, yRange);

        transform.localPosition = new Vector3(xLimiter, yLimiter, transform.localPosition.z);
    }

    private void Rotation()
    {
        float pitch = transform.localPosition.y * positionPithcFactor + yThrow * controlPitchFactor;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void Firing()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunsActive(true);
        }
        else
        {
            SetGunsActive(false);
        }
    }

    private void SetGunsActive(bool isActive)
    {
        foreach (GameObject gun in guns)
        {
            ParticleSystem.EmissionModule shipGun = gun.GetComponent<ParticleSystem>().emission;
            shipGun.enabled = isActive;
        }
    }

}
