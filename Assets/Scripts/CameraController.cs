using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This script is attached to the camera using keyboard input to move the camera around the scene in Unity.
//  key W: Move camera forward
public class CameraController : MonoBehaviour
{
    //  The speed at which the camera moves
    public float speed = 10.0f;

    //  The speed at which the camera rotates
    public float rotationSpeed = 100.0f;

    void Update()
    {
        //  Move the camera forward
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        //  Move the camera backward
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        //  Move the camera left
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        //  Move the camera right
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        //  Rotate the camera horizontally using the mouse x-axis
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime);

        //  Rotate the camera vertically using the mouse y-axis
        transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime);

        //  Clamp the camera's rotation to prevent it from going upside down
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

    }



}



