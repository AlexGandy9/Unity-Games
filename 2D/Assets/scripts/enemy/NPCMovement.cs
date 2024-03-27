using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Playables;

public class NPCMovement : MonoBehaviour, IDamagable
{
    internal Transform thisTransform;
    public GameObject hitPoints;

    private bool attacking;

    public float moveSpeed;
    public float checkRadius;
    public float attackRadius;

    private bool attackAllowed;

    public Collider2D attackCollider;
    public float startTimeBtwAttack;

    public float startAttackRest;

    private float attackRest;

    private float timeBtwAttack;

    public static LayerMask whatIsPlayer;

    private bool kineFlag = true;

    public Transform player;
    private Color origColor;

    private Transform target;
    private Transform goalPos;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    public Vector3 dir;
    private bool chaseRange;
    private bool attackRange;

    public Transform[] path;
    public int currentPoint;


    public float health;
    public Slider healthBar;
    public GameObject healthBarObj;
    private float healthMetric;

    public float fadeDelay = 10f;
    public float alphaValue = 0f;
    public bool destroyGameObject = false;

    public bool killingTrigger;
    public GameObject trigger;

    public GameObject trigger2;

    public static NPCMovement INSTANCE;

    SpriteRenderer spriteRenderer;

    [SerializeField] public AudioSource swordSwing;
    [SerializeField] public AudioSource enemyVoice;

    private PlayableDirector cutscene1;

    public bool choice;
    public string decider;


    // Use this for initialization
    void Start()
    {

        attacking = false;

        attackAllowed = true;

        INSTANCE = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        thisTransform = this.transform;
        healthMetric = health;

        whatIsPlayer = LayerMask.GetMask("Player");

    }

    void Update()
    {

        Vector3 newPos = new Vector3(thisTransform.position.x, thisTransform.position.y - (18 / 20), 0);

        chaseRange = Physics2D.OverlapCircle(newPos, checkRadius, whatIsPlayer);
        attackRange = Physics2D.OverlapCircle(newPos, attackRadius, whatIsPlayer);

        dir = target.position - thisTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();

        bool flipped = movement.x < 0;
        this.thisTransform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));

    }

    private void FixedUpdate()
    {

        if (timeBtwAttack <= 0)
        {
            anim.SetBool("Attack", false);
            attacking = false;

            if (attackRest <= 0)
            {
                attackAllowed = true;
            }
            else
            {
                attackAllowed = false;
                attackRest -= Time.deltaTime;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
            attackAllowed = false;
            attackRest -= Time.deltaTime;
        }

        if (chaseRange && !attackRange)
        {
            enemyVoice.Play();
            // anim.SetBool("Attack", false);
            if (!attacking)
            {
                rb.isKinematic = false;
                MoveCharacter(dir);
                anim.SetFloat("x", target.position.x - thisTransform.position.x);
                anim.SetFloat("y", target.position.y - thisTransform.position.y);
                movement = dir;
            }
        }
        else if (attackRange)
        {
            if (kineFlag)
            {
                rb.isKinematic = true;
            }
            if (attackAllowed)
            {
                swordSwing.Play();
                anim.SetBool("Attack", true);
                rb.isKinematic = true;
                timeBtwAttack = startTimeBtwAttack;
                attackRest = startAttackRest;
                attacking = true;
            }
            rb.velocity = Vector2.zero;
            anim.SetFloat("x", target.position.x - thisTransform.position.x);
            anim.SetFloat("y", target.position.y - thisTransform.position.y);
            movement = dir;
        }
        else if (Vector3.Distance(thisTransform.position, path[currentPoint].position) > 0.6)
        {
            rb.isKinematic = false;

            Vector3 temp = Vector3.MoveTowards(thisTransform.position,
            path[currentPoint].position,
            moveSpeed * Time.deltaTime);
            rb.MovePosition(temp);
            anim.SetFloat("x", transform.position.x - path[currentPoint].position.x);
            anim.SetFloat("y", transform.position.y - path[currentPoint].position.y);
            movement = temp - thisTransform.position;
        }
        else
        {
            rb.isKinematic = false;

            ChangeGoal();
        }
    }

    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                PlayerMovement.EnemyDead();
                rb.isKinematic = true;
                GetComponent<Animator>().enabled = false;
                StartCoroutine(FadeTo(alphaValue, fadeDelay));
                if(killingTrigger){
                    if(choice){
                        if(PlayerPrefs.GetInt(decider) == 1){
                            cutscene1 = trigger2.GetComponent<PlayableDirector>();
                        }
                        else{
                            cutscene1 = trigger.GetComponent<PlayableDirector>();
                        }
                    }
                    else{
                        cutscene1 = trigger.GetComponent<PlayableDirector>();
                    }
                    cutscene1.Play();
                }
            }
        }
        get
        {
            return health;
        }
    }

    private void MoveCharacter(Vector2 dir)
    {
        rb.MovePosition((Vector2)thisTransform.position + (dir * moveSpeed * Time.deltaTime));
    }


    private void ChangeGoal()
    {
        if (currentPoint == path.Length - 1)
        {
            currentPoint = 0;
        }
        else
        {
            currentPoint++;
        }
    }
    public void OnHit(float damage, Vector2 knockback)
    {
        GameObject points = Instantiate(hitPoints, transform.position, Quaternion.identity);
        points.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
        kineFlag = false;
        rb.isKinematic = false;
      
        //knockback
        rb.AddForce(knockback);

        Health -= damage;

        


        healthBar.value = health / healthMetric;

        //stop any attack animation 
        anim.SetBool("Attack", false);

        // flash red
        origColor = spriteRenderer.material.color;

        spriteRenderer.material.color = Color.red;
        Invoke("FlashStop", 0.1f);
    }

    void FlashStop()
    {
        spriteRenderer.material.color = origColor;
    }

    IEnumerator FadeTo(float aValue, float fadeTime)
    {
        rb.simulated = false;

        float alpha = spriteRenderer.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(alpha, aValue, t));
            spriteRenderer.color = newColor;
            yield return null;
        }
        attackCollider.enabled = false;

        if (health <= 0)
        {
            gameObject.SetActive(false);
            healthBarObj.SetActive(false);
        }
    }

    public void switchKine(bool kin)
    {
        rb.isKinematic = kin;
        kineFlag = kin;
    }
}
