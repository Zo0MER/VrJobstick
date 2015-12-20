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
    public float maxSpeed = 1f;
    Vector3 moveDirection;
    public float gravity = 9.8f;
    public Text[] axis;
    public Button calibrate;

    Vector3 delta = new Vector3(9999, 0, 9999);

    public bool rotateByLean = false;

    bool isCalibrated = false;

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
        calibrate.transform.localScale = Vector3.zero;
        isCalibrated = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(0, 0, 0);

        // MoveForward(CrossPlatformInputManager.GetAxis("Vertical"));
        // MoveRight(CrossPlatformInputManager.GetAxis("Horizontal"));

        JobstickAngle angle = Jobstick.controller.GetAnglesToPlayerFromAddress(addressJobstick);
        if (angle != null)
        {

            string info = string.Format("{0}\nax = {1}\nay = {2}\naz = {3}", addressJobstick, angle.fixedX, angle.fixedY, angle.fixedZ);
            Debug.Log(info);    
            if (!rotateByLean)// && isCalibrated)
            {

                moveDirection.x = angle.fixedX;// - delta.x;
                moveDirection.z = angle.fixedY;// - delta.z;


                //if (Mathf.Abs(angle.fixedX) < 3) moveDirection.x = 0;
                //if (Mathf.Abs(angle.fixedY) < 3) moveDirection.z = 0;

                axis[0].text = "fx = " + angle.fixedX.ToString();
                axis[1].text = "fy = " + angle.fixedY.ToString();



                //moveDirection = transform.TransformDirection(moveDirection)* maxSpeed;

                axis[2].text = "x = " + moveDirection.x.ToString();
                axis[3].text = "y = " + moveDirection.z.ToString();

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
        characterController.Move(moveDirection * maxSpeed * Time.deltaTime);
    }
}
