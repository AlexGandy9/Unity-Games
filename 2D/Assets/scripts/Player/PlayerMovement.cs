using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 3f;

    private bool playerDeathSwitch;

    public Rigidbody2D rb;

    private Color origColor;

    public Animator animator;

    public bool canMove;

    public Transform movePoint;
    private static bool endingKey = true;

    public float timeForMovePoint;

    public float startInvincibility = 0.2f;
    private float invinsibilty;

    public static PlayerMovement INSTANCE;

    //Health bar variables
    public static float healthStart = 3f;
    public static float health = 3f;
    public Slider healthBar;

    //Darkness bar variables
    public static float darkness = 0;
    public Slider darknessBar;
    //Keeps track to add to darkness
    public static int enemiesKilled = 0;

    //Shield bar variables
    public static float shield = 1;
    public Slider shieldBar;
    public AudioSource shieldSound;

    //Evade bar variables
    public static float evade = 1f;
    public Slider evadeBar;

    public float fadeDelay = 1f;
    public float alphaValue = 0f;
    public bool destroyGameObject = false;

    public VectorValue startingPos;

    public Vector2 startingPos2;

    SpriteRenderer spriteRenderer;

    private float time = 0;
    private float time2 = 0;
    private bool playerStealth = false;

    public static bool bronze = false;

    public bool isPot;
    public bool isDemon;

    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                StartCoroutine(FadeTo(alphaValue, fadeDelay, false));

            }
        }
        get
        {
            return health;
        }
    }


    public GameObject playerAttackHitbox;

    Collider2D swordCollider;

    Vector2 movement;

    private bool isAllowed;

    private static   Vector2 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        playerDeathSwitch = true;
        
        transform.position = new Vector3(startingPos.initialValue.x, startingPos.initialValue.y, -3);
        movePoint.transform.position = new Vector3(startingPos.initialValue.x, startingPos.initialValue.y, -3);
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        origColor = spriteRenderer.material.color;
        canMove = true;
        INSTANCE = this;
        isAllowed = true;
        swordCollider = playerAttackHitbox.GetComponent<Collider2D>();
        rb.isKinematic = false;
        healthBar.value = health / healthStart;
        darknessBar.value = darkness;
        PlayerAttack.damage = (int)(darkness * 5);
        if(bronze){
            PlayerAttack.damage += 1;
        }
        Debug.Log(bronze);

        initialPosition = this.transform.position;
        evadeBar.value = 1;
    }
    private void Load()
    {
        this.transform.position = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        evadeBar.value = evade;
        if (isPot)
        {
            moveSpeed = 3.5f;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && !isPot)
        {
            animator.speed = 2;
            moveSpeed = 7f;
        }
        else
        {
            moveSpeed = 4f;
            animator.speed = 1;
        }
        if (isDemon)
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                animator.transform.SetPositionAndRotation(this.transform.position, new Quaternion(0, 0, 0, 0));
            }else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                animator.transform.SetPositionAndRotation(this.transform.position, new Quaternion(0, 180, 0, 0));
            }
        }
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");

            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("horizontal", movement.x);
            animator.SetFloat("vertical", movement.y);
            animator.SetFloat("speed", movement.sqrMagnitude);

            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1
            || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                animator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (evade > 0)
            {
                evade -= 0.5f;
                evadeBar.value = evade;
                StartCoroutine(FadeTo(0.3f, 0.2f, true));
                playerStealth = true;
                gameObject.tag = "Invisible";
                NPCMovement.whatIsPlayer = LayerMask.GetMask("Nothing");
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            shield -= 0.0015f;
            shieldSound.Play();
            shieldBar.value = shield;
            gameObject.tag = "Untagged";
            spriteRenderer.material.color = new Color(0.2f, 0.7f, 1f);
            if (shieldBar.value == 0)
            {
                shield = 0;
                spriteRenderer.material.color = Color.white;
                gameObject.tag = "Player";
            }
        }else
        {
            if (shield < 1)
            {
                shield += 0.00025f;
            }
            shieldBar.value = shield;
            spriteRenderer.material.color = Color.white;
            gameObject.tag = "Player";
        }

        if (playerStealth)
        {
            time += Time.deltaTime;
        }else if (evade < 1)
        {
            time2 += Time.deltaTime;
        }
        
        if (time2 >= 30)
        {
            evade += 0.5f;
            evadeBar.value = evade;
            time2 = 0;
        }
        if (time >= 5)
        {
            playerStealth = false;
            time = 0;
            gameObject.tag = "Player";
            NPCMovement.whatIsPlayer = LayerMask.GetMask("Player");
            StartCoroutine(FadeTo(1f, 0.2f, true));
        }

        if (darkness == 1)
        {
            if(endingKey){
            endingKey = false;
            SceneManager.LoadScene("endingBad");
            }
        }

        // if (health == 0)
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // }
    }
    void FixedUpdate()
    {
        darknessBar.value = darkness;
        PlayerAttack.damage = (int)(darkness * 4);
        if(bronze){
            PlayerAttack.damage += 1;
        }

        invinsibilty -= Time.deltaTime;

        if (timeForMovePoint <= 0)
        {
            movePoint.transform.position = transform.position;
        }
        else
        {
            timeForMovePoint -= Time.deltaTime;

        }

        if (Vector3.Distance(transform.position, movePoint.transform.position) > 0)
        {

            Vector3 temp = Vector3.MoveTowards(transform.position,
            movePoint.transform.position,
            70f * Time.deltaTime);
            rb.MovePosition(temp);

        }
        else
        {

            if (canMove)
            {
                if (isAllowed)
                {
                    rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
                }
            }

        }
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        if(invinsibilty <= 0){
            invinsibilty = startInvincibility;
      
            health -= damage;
            healthBar.value = health / healthStart;

            spriteRenderer.material.color = Color.red;
            movePoint.transform.position = new Vector2(transform.position.x + knockback.x, transform.position.y + knockback.y);
            timeForMovePoint = 0.3f;
            canMove = false;
            GetComponent<Animator>().enabled = false;
            if (health <= 0)
            {
                StartCoroutine(FadeTo(alphaValue, fadeDelay, false));
                if(playerDeathSwitch){
                    playerDeathSwitch = false;
                    health = healthStart;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            else
            {
                Invoke("FlashStop", 0.2f);
                Invoke("MoveAgain", 0.2f);
            }
        }
        
    }

    void FlashStop()
    {
        spriteRenderer.material.color = origColor;
    }
    void MoveAgain()
    {
        GetComponent<Animator>().enabled = true;
        canMove = true;
    }

    IEnumerator FadeTo(float aValue, float fadeTime, bool sim)
    {
        rb.simulated = sim;

        float alpha = spriteRenderer.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(alpha, aValue, t));
            spriteRenderer.color = newColor;
            yield return null;
        }
        if (destroyGameObject)
        {
            Destroy(gameObject);
        }

    }

    public static void EnemyDead()
    {
        Debug.Log("enemy" + enemiesKilled);
        enemiesKilled++;
        if (enemiesKilled == 3)
        {
            darkness += 0.09f;
            enemiesKilled = 0;
        }
        else{
            darkness += 0.08f;
        }
    }

}


