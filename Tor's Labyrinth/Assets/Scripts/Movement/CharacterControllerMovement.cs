using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    //Camera
    private Vector2 PlayerMouseInput;
    private float xRot;
    private bool targetsSet;
    private Quaternion targetRotation;
    private Vector3 targetPosition;
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Transform PlayerEyes;
    [SerializeField] private float Sensitivity;


    //Movement
    private Vector3 PlayerMovementInput;
    public static bool isSneaking = false;
    private float timeDelay = 0f;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private float Speed;
    [SerializeField] private static Transform playerTransform;


    //Jumping
    private Vector3 Velocity;
    private bool jump = false;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Gravity = 9.81f;


    //Audio
    [SerializeField] private AudioSource jumping;
    [SerializeField] private AudioSource walking;
    [SerializeField] private AudioSource hitPlayer;
    [SerializeField] private AudioSource trapSound;


    //Animation
    [SerializeField] private Animator animator;


    //Canvases
    public static Canvas winScreen;
    [SerializeField] private Canvas killScreen;
    [SerializeField] private Canvas winScreenHolder;
    [SerializeField] private GameObject positionArrow;


    void Awake()
    {
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

        if (!KillPlayer.isKilled && !KillPlayerMP.isKilled)
        { 
            if (!HitTrap2.isStuck)
            {
                MovePlayer();
                if (!HitTrap.isSlowed){
                    positionArrow.SetActive(false);
                }
            }
            else 
            {
                if (!positionArrow.active){
                    trapSound.Play();
                }
                positionArrow.SetActive(true);
                animator.SetFloat("Velocity X", 0f);
                animator.SetFloat("Velocity Z", 0f);
            }
            MovePlayerCamera();
        }
        else 
        {
            if (!targetsSet)
            {
                hitPlayer.Play();
                animator.SetBool("isKilled", true);
                targetPosition = PlayerCamera.transform.position + new Vector3(0, 3, 0);
                targetRotation = new Quaternion(0, 0, 0, 0);
                targetsSet = true;
                PlayerCamera.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }

        if (targetsSet)
        {
            PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, targetPosition, Time.deltaTime * 1);
        }

        if (PlayerCamera.transform.position == targetPosition)
        {
            Cursor.visible = true;
            killScreen.gameObject.SetActive(true);
        }
    }

    private void MovePlayer()
    {
        if (!Hide.hiding){
            Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

            if(Controller.isGrounded)
            {
                Velocity.y = -1f;
                if (jump == true)
                {
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

            if (!HitTrap.isSlowed)
            {
                if (!HitTrap2.isStuck){
                    positionArrow.SetActive(false);
                }
                
                if(Input.GetKey(KeyCode.X))
                {
                    Speed = 3f;
                    isSneaking = true;
                    animator.SetBool("isSneaking", true);
                    AnimateCharacter(MoveVector);
                }
                else if (!Input.GetKey(KeyCode.X) && GetInput())
                {
                    Speed = 5.5f;
                    PlayWalkSound(MoveVector);
                    isSneaking = false;
                    animator.SetBool("isSneaking", false);
                    AnimateCharacter(MoveVector);
                }
                else 
                {
                    animator.SetFloat("Velocity X", 0f);
                    animator.SetFloat("Velocity Z", 0f);
                    Speed = 0f;
                }
            } 
            else if (HitTrap.isSlowed)
            {
                if (!positionArrow.active){
                    trapSound.Play();
                }
                positionArrow.SetActive(true);
                Speed = 2f;
            }

            Controller.Move(MoveVector * Speed * Time.deltaTime);
            Controller.Move(Velocity * Time.deltaTime);
        }
        else 
        {
            animator.SetFloat("Velocity X", 0f);
            animator.SetFloat("Velocity Z", 0f); 
        }
    }

    private bool GetInput()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }

    private void AnimateCharacter(Vector3 MoveVector)
    {
        if (MoveVector == Vector3.zero)
        {
            timeDelay = 0f;
        }
        else if (timeDelay <= 1)
        {
            timeDelay += Time.deltaTime * 3.33f;
        }

        Vector3 animFloats = Vector3.Normalize(MoveVector);
        animFloats = playerTransform.InverseTransformDirection(animFloats);
        animator.SetFloat("Velocity X", animFloats.x * timeDelay);
        animator.SetFloat("Velocity Z", animFloats.z * timeDelay);
    }

    private void PlayWalkSound(Vector3 MoveVector)
    {
        if(MoveVector.magnitude > 0 && !walking.isPlaying && Controller.isGrounded)
        {
            walking.Play();
        }
        else if (!Controller.isGrounded || MoveVector.magnitude <= 0)
        {
            walking.Stop();
        }
    }

    private void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;
        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        if (xRot > -83f && xRot < 83f)
        {
            PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }
        else 
        {
            xRot += PlayerMouseInput.y * Sensitivity;
        }

        PlayerCamera.transform.position = PlayerEyes.transform.position + PlayerCamera.transform.TransformDirection(new Vector3(0, 0, 0.2f));
    }

    public static void SetPos(Transform pos)
    {
        playerTransform.position = pos.position;
    }
}
