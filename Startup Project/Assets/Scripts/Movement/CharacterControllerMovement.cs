using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{

    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private float xRot;
    private Vector2 PlayerMouseInput;

    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private CharacterController Controller;

    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float Gravity = 9.81f;
    [SerializeField] private static Transform playerTransform;

    public static bool isSneaking = false;

    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); 
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        playerTransform = transform;

        if (!HitTrap2.isStuck){
            MovePlayer();
        }
        MovePlayerCamera();

        if(!isSneaking){
            //play walking sound
        }
    }

    private void MovePlayer()
    {
        if (!Hide.hiding){
            Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

            if(Controller.isGrounded)
            {
                Velocity.y = -1f;

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    Velocity.y = JumpForce;
                }
            }
            else
            {
                Velocity.y -= Gravity * 2f * Time.deltaTime;
            }

            if (!HitTrap.isSlowed){
                if(Input.GetKeyDown(KeyCode.X)){
                    Speed = 3f;
                    isSneaking = true;
                }else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyUp(KeyCode.X)){
                    Speed = 6f;
                    isSneaking = false;
                }
            } else if (HitTrap.isSlowed){
                Speed = 2f;
                isSneaking = true;
            }
            Controller.Move(MoveVector * Speed * Time.deltaTime);
            Controller.Move(Velocity * Time.deltaTime);
        }
    }

    private void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;
        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        if (xRot > -83f && xRot < 83f){
            PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }else {
            xRot += PlayerMouseInput.y * Sensitivity;
        }
    }

    public static void SetPos(Transform pos){
        playerTransform.position = pos.position;
    }
}
