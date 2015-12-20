using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using JobstickSDK;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {

    public GameObject howerboard;
    public float leanAngle = 45.0f;
    CharacterController characterController;
    public float maxSpeed = 1.0f;
    Vector3 moveDirection;
    public float gravity = 9.8f;

    public bool rotateByLean = false;

    public string addressJobstick;

    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        // MoveForward(CrossPlatformInputManager.GetAxis("Vertical"));
        // MoveRight(CrossPlatformInputManager.GetAxis("Horizontal"));

        JobstickAngle angle = Jobstick.controller.GetAnglesToPlayerFromAddress(addressJobstick);
        if (angle != null)
        {
            string info = string.Format("{0}\nax = {1}\nay = {2}\naz = {3}", addressJobstick, angle.ax, angle.ay, angle.az);

            Debug.Log(info);
        }

            if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            moveDirection = new Vector3(0, 0, CrossPlatformInputManager.GetAxis("Vertical"));

            if (!rotateByLean )
            {
                transform.Rotate(0, CrossPlatformInputManager.GetAxis("Rotate"), 0);
                moveDirection.x = CrossPlatformInputManager.GetAxis("Horizontal");

                howerboard.transform.localRotation = Quaternion.Euler(
                   270.0f + CrossPlatformInputManager.GetAxis("Vertical") * leanAngle, 0.0f,
                   CrossPlatformInputManager.GetAxis("Horizontal") * leanAngle);
            }
            else
            {
                transform.Rotate(0, CrossPlatformInputManager.GetAxis("Horizontal"), 0);
                howerboard.transform.localRotation = Quaternion.Euler(
                   270.0f + CrossPlatformInputManager.GetAxis("Vertical") * leanAngle, 0.0f,
                   CrossPlatformInputManager.GetAxis("Horizontal") * leanAngle);
            }

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= maxSpeed;

        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
