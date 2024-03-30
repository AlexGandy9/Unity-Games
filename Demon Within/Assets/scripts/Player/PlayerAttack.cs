  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public static int damage = 0;

    public bool talking;

    public Animator animator2;

    public GameObject enemyToHit;

    private bool flag1 = false;
    public static PlayerAttack INSTANCE;

    [SerializeField] public AudioSource swingSword;

    public bool doorCheck = false;


    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        talking = false;
        timeBtwAttack = 0;


    }

    void Update()
    {
     

        if (timeBtwAttack <= 0)
        {

            if (flag1)
            {
                flag1 = false;
                 if (GameObject.Find("enemies") != null){
                    NPCMovement.INSTANCE.switchKine(true);
                 } 
            }
            if (!talking)
            {
                if(!doorCheck){
                    PlayerMovement.INSTANCE.canMove = true;

                }
                
                animator2.SetBool("isAttacking", false);

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.P))
                {
                    swingSword.Play();
                    if (GameObject.Find("enemies") != null){
                         NPCMovement.INSTANCE.switchKine(false);
                    }
                    else{
                    flag1 = true;

                    }

                    PlayerMovement.INSTANCE.canMove = false;
                    timeBtwAttack = startTimeBtwAttack;
                    animator2.SetBool("isAttacking", true);
                }
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

    }
}
