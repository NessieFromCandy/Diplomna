using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




[RequireComponent(typeof(CharacterController))]
public class Player_movement : MonoBehaviour
{

    public Camera playerCam;
    public float WalkSpeed = 6f;
    public float RunSpeed = 12f;
    public float JumpPower = 7f;
    public float Gravity = 10f;

    public float LookSpeed = 2f;
    public float LookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool CanMove = true;
    

    CharacterController characterController;

    void Start()
    {

        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        //Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = CanMove ? (isRunning ? RunSpeed : WalkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = CanMove ? (isRunning ? RunSpeed : WalkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);




        //Jumping
        if (Input.GetButton("Jump") && CanMove && characterController.isGrounded)
        {
            moveDirection.y = JumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        if (!characterController.isGrounded)
        {
            moveDirection.y -= Gravity * Time.deltaTime;
        }



        //Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (CanMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * LookSpeed;
            rotationX = Mathf.Clamp(rotationX, -LookXLimit, LookXLimit);
            playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * LookSpeed, 0);


        }

    }


       
    

    

}
