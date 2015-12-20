using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using JobstickSDK;
using UnityEngine.UI;
[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{

    public GameObject howerboard;
    public float leanAngle = 45.0f;
    CharacterController characterController;
    public float maxSpeed = 1.0f;
    Vector3 moveDirection;
    public float gravity = 9.8f;
    public Text[] axis;

    Vector3 delta = new Vector3(9999, 9999);

    public bool rotateByLean = false;

    public string addressJobstick;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Calibrate()
    {
        JobstickAngle angle = Jobstick.controller.GetAnglesToPlayerFromAddress(addressJobstick);
        if (angle != null)
        {
            delta = new Vector3(angle.fixedX, 0.0f, angle.fixedY);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(0, 0, 0);

        // MoveForward(CrossPlatformInputManager.GetAxis("Vertical"));
        // MoveRight(CrossPlatformInputManager.GetAxis("Horizontal"));

        JobstickAngle angle = Jobstick.controller.GetAnglesToPlayerFromAddress(addressJobstick);
        if (angle != null && delta == new Vector3(9999, 9999))
        {

            string info = string.Format("{0}\nax = {1}\nay = {2}\naz = {3}", addressJobstick, angle.fixedX, angle.fixedY, angle.fixedZ);
            Debug.Log(info);    
            if (!rotateByLean)
            {

                moveDirection -= delta;

                if (angle.fixedX < 5) moveDirection.x = 0;
                if (angle.fixedZ < 5) moveDirection.z = 0;

                axis[0].text = angle.fixedX.ToString();
                axis[1].text = angle.fixedY.ToString();
                axis[2].text = angle.fixedZ.ToString();
                axis[3].text = moveDirection.x.ToString();
                axis[4].text = moveDirection.y.ToString();
                axis[5].text = moveDirection.z.ToString();


                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= maxSpeed;

                //howerboard.transform.localRotation = Quaternion.Euler(
                //   270.0f + CrossPlatformInputManager.GetAxis("Vertical") * leanAngle, 0.0f,
                //   CrossPlatformInputManager.GetAxis("Horizontal") * leanAngle);
            }
            else
            {
                transform.Rotate(0, CrossPlatformInputManager.GetAxis("Horizontal"), 0);
                //howerboard.transform.localRotation = Quaternion.Euler(
                //   270.0f + CrossPlatformInputManager.GetAxis("Vertical") * leanAngle, 0.0f,
                //   CrossPlatformInputManager.GetAxis("Horizontal") * leanAngle);
            }
            Debug.Log(info);
        }


        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
