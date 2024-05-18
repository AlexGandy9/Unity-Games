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
    private Vector3 cameraPos;


    //Movement
    private Vector3 PlayerMovementInput;
    public static bool isSneaking = false;
    private float timeDelay = 0f;
    private float timeDelay2 = 0f;
    private float timeDelayA = 0f;
    private float timeDelayW = 0f;
    private float timeDelayS = 0f;
    private float timeDelayD = 0f;
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
    [SerializeField] public Animator animator;


    //Canvases
    public static Canvas winScreen;
    [SerializeField] private Canvas killScreen;
    [SerializeField] private Canvas winScreenHolder;
    [SerializeField] private GameObject positionArrow;

    private Vector3 pmi = Vector3.zero;


    void Awake()
    {
        winScreen = winScreenHolder;
        Cursor.visible = false;
        KillPlayer.isKilled = false;
        KillPlayerMP.isKilled = false;
        KillPlayerMove.isKilled = false;
 
    }
    void Player1Movement()
    {
        /*if (!GetInput()){
            if (timeDelay2 > 0f){
                timeDelay2 -= Time.deltaTime * 0.37f;
            }else {
                timeDelay2 = 0f;
            }
            pmi.z = pmi.z * timeDelay2;
            pmi.x = pmi.x * timeDelay2;
        }else {
            if (timeDelay2 < 0.95f){
                pmi = Vector3.zero;
                timeDelay2 += Time.deltaTime * 2.5f;
            }else {
                pmi = Vector3.zero;
                timeDelay2 = 1f;
            }
        }


        if (Input.GetKey(KeyCode.A))
        {
            pmi.x += -1f * timeDelay2;

        }

        if (Input.GetKey(KeyCode.D))
        {
            pmi.x += 1f * timeDelay2;

        }

        if (Input.GetKey(KeyCode.W))
        {
            pmi.z += 1f * timeDelay2;

        }

        if (Input.GetKey(KeyCode.S))
        {
            pmi.z += -1f * timeDelay2;
        }*/
        
        if (Input.GetKey(KeyCode.A))
        {
            if (timeDelayA < 0.95f){
                timeDelayA += Time.deltaTime * 2.5f;
            }else {
                timeDelayA = 1f;
            }

        }else{
            if (timeDelayA > 0f){
                timeDelayA -= Time.deltaTime * 5;
            }else {
                timeDelayA = 0f;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (timeDelayD < 0.95f){
                timeDelayD += Time.deltaTime * 2.5f;
            }else {
                timeDelayD = 1f;
            }

        }else{
            if (timeDelayD > 0f){
                timeDelayD -= Time.deltaTime * 5;
            }else {
                timeDelayD = 0f;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (timeDelayW < 0.95f){
                timeDelayW += Time.deltaTime * 2.5f;
            }else {
                timeDelayW = 1f;
            }

        }else{
            if (timeDelayW > 0f){
                timeDelayW -= Time.deltaTime * 5;
            }else {
                timeDelayW = 0f;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (timeDelayS < 0.95f){
                timeDelayS += Time.deltaTime * 2.5f;
            }else {
                timeDelayS = 1f;
            }

        }else{
            if (timeDelayS > 0f){
                timeDelayS -= Time.deltaTime * 5;
            }else {
                timeDelayS = 0f;
            }
        }

        pmi = Vector3.zero;
        pmi.x += -1f * timeDelayA;
        pmi.x += 1f * timeDelayD;
        pmi.z += 1f * timeDelayW;
        pmi.z += -1f * timeDelayS;
    }

    // Update is called once per frame
    void Update()
    {
        Player1Movement();
        PlayerMovementInput = pmi;
        //PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); 
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        playerTransform = transform;

        if (!KillPlayer.isKilled && !KillPlayerMP.isKilled && !KillPlayerMove.isKilled)
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

            if (PlayerCamera.transform.position == targetPosition)
            {
                Cursor.visible = true;
                killScreen.gameObject.SetActive(true);
            }
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
                    timeDelay = 0.5f;
                    animator.SetBool("isJumping", false);
                    if (!winScreen.gameObject.active){
                        jumping.Play();
                    }
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
                    Speed = 6f;
                    PlayWalkSound(MoveVector);
                    isSneaking = false;
                    animator.SetBool("isSneaking", false);
                    AnimateCharacter(MoveVector);
                }
                else 
                {
                    animator.SetFloat("Velocity X", pmi.x);
                    animator.SetFloat("Velocity Z", pmi.z);
                }
            } 
            else if (HitTrap.isSlowed)
            {
                if (!positionArrow.active){
                    trapSound.Play();
                }
                positionArrow.SetActive(true);
                Speed = 2f;
                AnimateCharacter(MoveVector);
            }

            if (MoveVector.magnitude > 1f){
                MoveVector = MoveVector.normalized;
            }
            Controller.Move(MoveVector * Speed * Time.deltaTime);
            Controller.Move(Velocity * Time.deltaTime);
        }
        else 
        {
            animator.SetFloat("Velocity X", pmi.x);
            animator.SetFloat("Velocity Z", pmi.z);
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
            timeDelay += Time.deltaTime * 3.33333f;
        }

        Vector3 animFloats = Vector3.Normalize(MoveVector);
        animFloats = playerTransform.InverseTransformDirection(animFloats);
        animator.SetFloat("Velocity X", pmi.x);
        animator.SetFloat("Velocity Z", pmi.z);

    }

    private void PlayWalkSound(Vector3 MoveVector)
    {
        if(MoveVector.magnitude > 0 && !walking.isPlaying && Controller.isGrounded && !winScreen.gameObject.active)
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
        if (xRot > -75f && xRot < 80f)
        {
            PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }
        else 
        {
            xRot += PlayerMouseInput.y * Sensitivity;
        }

        PlayerCamera.transform.position = PlayerEyes.transform.position + PlayerEyes.transform.TransformDirection(new Vector3(0, -0.185f, 0f));
    }

    public static void SetPos(Transform pos)
    {
        playerTransform.position = pos.position;
    }
}
