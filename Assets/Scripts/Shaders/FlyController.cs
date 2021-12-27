using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to control the fly like aeroplane.
// It is attached to the aeroplane prefab.
// The script uses the aeroplane's rigidbody to control the plane.
// The rigidbody is moved by the player input.

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class FlyController : MonoBehaviour
{
    // The aeroplane rigidbody
    // Aeroplane variables
    public float m_Speed = 10f; // How fast the aeroplane moves forward and backward
    // variable - target speed to fly
    public float targetFlySpeed = 10f;
    // variable - max speed to fly
    public float maxFlySpeed = 20f;
    public float m_TurnSpeed = 180f; // How fast the aeroplane turns left and right
    public float m_PitchUpperLimit = 60f; // The maximum pitch the aeroplane can be moved to
    public float m_PitchLowerLimit = -30f; // The minimum pitch the aeroplane can be moved to
    public float m_RollSpeed = 180f; // How fast the aeroplane rolls to a side
    public float m_PitchInput = 0f; // The amount that the aeroplane is pitched
    public float m_RollInput = 0f; // The amount that the aeroplane is rolled
    public float m_YawInput = 0f; // The amount that the aeroplane is yawed
    public float m_ThrottleInput = 0f; // The amount that the aeroplane is throttled
    public float m_BrakeInput = 0f; // The amount that the aeroplane is braked

    // The aeroplane's rigidbody and the aeroplane's transform
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;

    void Start()
    {
        // make camera dont lost visual after rotating
       // Camera.main.GetComponent<Camera>().cullingMask = ~(1 << 8); 
        // Set the rigidbody and the transform
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();
    }

    void Update()
    {
        // Get the input for the pitch, yaw, roll and throttle of the aeroplane
        m_PitchInput = Input.GetAxis("Vertical");
        m_YawInput = Input.GetAxis("Horizontal");
        m_RollInput = Input.GetAxis("Roll");
        m_ThrottleInput = Input.GetAxis("Throttle");
        m_BrakeInput = Input.GetAxis("Brake");
    }

    void FixedUpdate()
    {
        // Move and rotate the aeroplane based on the player input
        Move();
        Rotate();
    }

    // Moves the aeroplane forward and backward
    void Move()
    {
        targetFlySpeed += m_ThrottleInput;  // Add ThrottleInput to the target speed
        targetFlySpeed = Mathf.Clamp(targetFlySpeed, 0f, maxFlySpeed);  // Clamp the target speed between minimum and maximum speeds

        // Add throttle input to forward movement
        Vector3 movement = transform.forward * targetFlySpeed * Time.deltaTime;

        // Change the position of the aeroplane based on the movement vectors
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    // Rotates the aeroplane left and right
    void Rotate()
    {
        // Determine the aeroplane's pitch, yaw and roll
        float pitch = m_PitchInput * m_PitchInput * m_PitchUpperLimit * Time.deltaTime;
        float yaw = m_YawInput * m_TurnSpeed * Time.deltaTime;
        float roll = m_RollInput * m_RollSpeed * Time.deltaTime;

        // Create rotation quaternions
        Quaternion pitchRotation = Quaternion.Euler(pitch, 0f, 0f);
        Quaternion yawRotation = Quaternion.Euler(0f, yaw, 0f);
        Quaternion rollRotation = Quaternion.Euler(0f, 0f, roll);

        // Rotate the aeroplane based on the rotation quaternions
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * pitchRotation);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * yawRotation);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * rollRotation);
    }

    // This function is called when the behaviour becomes disabled or inactive
    void OnDisable()
    {
        // Reset the pitch, yaw and roll input
        m_PitchInput = 0f;
        m_YawInput = 0f;
        m_RollInput = 0f;
        m_ThrottleInput = 0f;
        m_BrakeInput = 0f;
    }

    // This function is called when the behaviour is destroyed
    void OnDestroy()
    {
        // Reset the pitch, yaw and roll input
        m_PitchInput = 0f;
        m_YawInput = 0f;
        m_RollInput = 0f;
        m_ThrottleInput = 0f;
        m_BrakeInput = 0f;
    }

    

}