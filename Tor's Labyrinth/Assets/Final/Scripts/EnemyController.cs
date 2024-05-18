using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    //Movement
    [SerializeField] private CharacterController Controller;
    private Vector3 MoveVector;
    private Vector3 MoveVector2;
    private Vector3 placeVector;
    private float Speed;
    public float baseSpeed;
    public float attackSpeed;

    //Jumping
    private Vector3 Velocity = new Vector3(0, -1f, 0);
    private bool jump = false;
    private float Gravity = 9.81f;

    //Attacking

    //camera
    private float xRot;
    private float ySensitivity = 3f;
    private float xSensitivity = 6f;
    private Vector2 CamMoveVector;
    [SerializeField] Camera PlayerCamera;
    [SerializeField] GameObject PlayerEyes;
    //Camera position used when player is attacking (jittery camera movement otherwise)
    [SerializeField] GameObject AttackEyes;

    //Input handlers
    private PlayerInput playerInput;
    private InputAction movement;
    private InputAction cameraMovement;

    //Audio and animation
    private float timeDelay = 0f;
    public static Animator animator;
    [SerializeField] AudioSource jumping;
    [SerializeField] AudioSource attack;
    [SerializeField] AudioSource walkSound;

    private void Awake(){
        playerInput = new PlayerInput();
        PlayerCamera.transform.localRotation = new Quaternion(0, 0, 0, 0);
        animator = GetComponent<Animator>();

        Speed = baseSpeed;
    }

    private void OnEnable(){
        movement = playerInput.Enemy.Walk;
        movement.Enable();

        playerInput.Enemy.Jump.performed += DoJump;
        playerInput.Enemy.Jump.Enable();

        playerInput.Enemy.Attack.performed += DoAttack;
        playerInput.Enemy.Attack.Enable();

        cameraMovement = playerInput.Enemy.MoveCamera;
        cameraMovement.Enable();
    }

    private void DoJump(InputAction.CallbackContext obj){
        if(Controller.isGrounded){
            Velocity.y = 8f;
            jump = true;
            animator.SetBool("isJumping", true);
            Controller.Move(Velocity * Time.deltaTime);
        }
    }
    private void DoAttack(InputAction.CallbackContext obj){
        if (!animator.GetBool("isAttacking")){
            attack.Play();
        }

        PlayerCamera.transform.position = AttackEyes.transform.position;
        animator.SetBool("isAttacking", true);
    }

    //Disable all inputs to prevent game loop errors
    private void OnDisbale(){
        movement.Disable();
        cameraMovement.Disable();
        playerInput.Enemy.Jump.Disable();
        playerInput.Enemy.Attack.Disable();
    }
 
    private void FixedUpdate(){
        //Sets initial moving vectors
        MoveVector = transform.TransformDirection(new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y));
        MoveVector2 = transform.TransformDirection(new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y));
        if (MoveVector.magnitude == 0f && placeVector.magnitude != 0f){
            placeVector = placeVector * timeDelay;
            MoveVector = placeVector;
        }else {
            placeVector = MoveVector;
        }
        CamMoveVector = cameraMovement.ReadValue<Vector2>();
        
        //Handles when player is the air and when they land
        if (jump && Controller.isGrounded){
            Velocity.y = -1f;
            animator.SetBool("isJumping", false);
            jumping.Play();
            jump = false;
        }else {
            Velocity.y -= Gravity * 2f * Time.deltaTime;
        }

        //Handles the attacking of the player
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack1")){
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f){
                animator.SetBool("isAttacking", false);
                Speed = attackSpeed;
                Invoke("ResetSpeed", 3f);
            }
        }
        PlayerCamera.transform.position = PlayerEyes.transform.position + PlayerEyes.transform.TransformDirection(new Vector3(0, 0, 0.2f));

        //Camera rotation and position
        xRot -= CamMoveVector.y * ySensitivity;
        transform.Rotate(0f, CamMoveVector.x * xSensitivity, 0f);
        if (xRot > -65f && xRot < 65f){
            PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }else {
            xRot += CamMoveVector.y * ySensitivity;
        }

        AnimateCharacter(MoveVector);

        //Character movement
        Controller.Move(new Vector3(MoveVector.x, 0, MoveVector.z) * Speed * Time.deltaTime * timeDelay);
        Controller.Move(Velocity * Time.deltaTime);
    }

    private void ResetSpeed(){
        Speed = baseSpeed;
    }

    private void AnimateCharacter(Vector3 MoveVector)
    {
        if (MoveVector == Vector3.zero || !Controller.isGrounded){
            walkSound.Stop();
        }else {
            if (!walkSound.isPlaying){
                walkSound.Play();
            }
        }

        if (MoveVector2 == Vector3.zero)
        {
            if (timeDelay > 0f){
                timeDelay -= Time.deltaTime;
            }else {
                timeDelay = 0f;
            }
        }
        else if (timeDelay <= 1)
        {
            timeDelay += Time.deltaTime * 3.33f;
        }

        Vector3 animFloats = Vector3.Normalize(MoveVector);
        animFloats = transform.InverseTransformDirection(animFloats);
        animator.SetFloat("Velocity X", animFloats.x * timeDelay);
        animator.SetFloat("Velocity Z", animFloats.z * timeDelay);
    }
}