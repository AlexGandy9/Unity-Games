using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovementMP : MonoBehaviour
{

    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private float xRot;
    private Vector2 PlayerMouseInput;

    [SerializeField] private Transform PlayerCamera;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool targetsSet;
    [SerializeField] private Transform PlayerEyes;
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float Gravity = 9.81f;
    [SerializeField] private static Transform playerTransform;
    [SerializeField] private CharacterController Controller;

    [SerializeField] private Animator animator;

    [SerializeField] private AudioSource jumping;
    [SerializeField] private AudioSource walking;
    [SerializeField] private AudioSource hitPlayer;
    [SerializeField] private Canvas killScreen;
    [SerializeField] private Canvas winScreenHolder;
    public static Canvas winScreen;
    public static bool isSneaking = false;
    private bool jump = false;

    private float timeDelay = 0f;

    void Awake(){
        winScreen = winScreenHolder;
        Cursor.visible = false;

        //Variable resets
        animator.SetBool("isKilled", false);
        targetsSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); 
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        playerTransform = transform;
        if (!KillPlayer.isKilled){ 
            if (!HitTrap2.isStuck){
                MovePlayer();
            }else {
                animator.SetFloat("Velocity X", 0f);
                animator.SetFloat("Velocity Z", 0f);
            }
            MovePlayerCamera();
        }else {
            if (!targetsSet){
                hitPlayer.Play();
                animator.SetBool("isKilled", true);
                targetPosition = PlayerCamera.transform.position + new Vector3(0, 3, 0);
                targetRotation = new Quaternion(0, 0, 0, 0);
                targetsSet = true;
                PlayerCamera.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
            //Change camera to view the player on the ground
            //give user the kill screen
            //In multiplayer give other player the kill screen
        }

        if (targetsSet){
            PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, targetPosition, Time.deltaTime * 1);
        }

        if (PlayerCamera.transform.position == targetPosition){
            Cursor.visible = true;
            killScreen.gameObject.SetActive(true);
        }
    }

    private void MovePlayer()
    {
        /*if (!Hide.hiding){
            Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

            if(Controller.isGrounded)
            {
                Velocity.y = -1f;
                if (jump == true){
                    animator.SetBool("isJumping", false);
                    jumping.Play();
                    jump = false;
                }

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetBool("isJumping", true);
                    Velocity.y = JumpForce;
                    jump = true;
                }
            }
            else
            {
                Velocity.y -= Gravity * 2f * Time.deltaTime;
            }

            if (!HitTrap.isSlowed){
                if(Input.GetKey(KeyCode.X)){
                    Speed = 3f;
                    isSneaking = true;
                    animator.SetBool("isSneaking", true);
                }else if (!Input.GetKey(KeyCode.X)){
                    Speed = 5.5f;
                    PlayWalkSound(MoveVector);
                    isSneaking = false;
                    animator.SetBool("isSneaking", false);
                }
                
                AnimateCharacter(MoveVector);

            } else if (HitTrap.isSlowed){
                Speed = 2f;
                isSneaking = true;
            }
            print(GetInput());
            if (GetInput()){
                Controller.Move(MoveVector * Speed * Time.deltaTime);
                Controller.Move(Velocity * Time.deltaTime);
            }
        }else {
            animator.SetFloat("Velocity X", 0f);
            animator.SetFloat("Velocity Z", 0f); 
        }*/
    }

    private bool GetInput(){
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }

    private void AnimateCharacter(Vector3 MoveVector){
        if (MoveVector == Vector3.zero){
            timeDelay = 0f;
        }else if (timeDelay <= 1){
            timeDelay += Time.deltaTime * 3.33f;
        }
        Vector3 animFloats = Vector3.Normalize(MoveVector);
        animFloats = playerTransform.InverseTransformDirection(animFloats);
        animator.SetFloat("Velocity X", animFloats.x * timeDelay);
        animator.SetFloat("Velocity Z", animFloats.z * timeDelay);
    }

    private void PlayWalkSound(Vector3 MoveVector){
        if(MoveVector.magnitude > 0 && !walking.isPlaying && Controller.isGrounded){
            walking.Play();
        }else if (!Controller.isGrounded || MoveVector.magnitude <= 0){
            walking.Stop();
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

        PlayerCamera.transform.position = PlayerEyes.transform.position;
    }

    public static void SetPos(Transform pos){
        playerTransform.position = pos.position;
    }
}
