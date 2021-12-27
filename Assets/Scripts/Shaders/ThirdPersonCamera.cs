using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Camera following the player
// Third person camera
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float height = 3.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    private float wantedRotationAngle;
    private float wantedHeight;
    private float currentRotationAngle;
    private float currentHeight;
    private Quaternion currentRotation;

    void LateUpdate()
    {
        wantedRotationAngle = target.eulerAngles.y;
        wantedHeight = target.position.y + height;
        currentRotationAngle = transform.eulerAngles.y;
        currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        transform.LookAt(target);
    }

}


