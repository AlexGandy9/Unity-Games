using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAIMoving : MonoBehaviour
{
    public static Animator animator;
    private NavMeshAgent agent;
    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource attackSound;

    //Attacking
    public float timeBetweenAttacks;
    public static bool alreadyAttacked;

    //States
    public float attackRange;
    public bool playerInAttackRange;
    private Vector3 enemyPosition;
    private Vector3 playerPosition;
    private float timeLoop = 0f;

    private void Awake(){
        alreadyAttacked = false;

        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("isKilled", false);
        enemyPosition = transform.position;
    }

    private void Update(){
        playerPosition = player.position;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        Vector3 animFloats = Vector3.Normalize(enemyPosition - transform.position);
        animFloats = transform.InverseTransformDirection(animFloats);
        animator.SetFloat("Velocity X", -animFloats.x);
        animator.SetFloat("Velocity Z", animFloats.z);

        if (!playerInAttackRange){
            ChasePlayer();
        }
        else if (playerInAttackRange){
            AttackPlayer();
        }
    }

    private void ChasePlayer(){
        agent.SetDestination(player.position);
        LookAtPlayer();
    }

    private void LookAtPlayer(){
        Vector3 relativePos = new Vector3(player.position.x, 0, player.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * 30);
    }

    private void AttackPlayer(){
        LookAtPlayer();
        agent.SetDestination(transform.position);
        if(!alreadyAttacked && !animator.GetBool("isKilled")){
            animator.SetBool("isAttacking", true);
            alreadyAttacked = true;
            Invoke("PlaySound", 0.1f);
            agent.speed = 4f;
            Invoke("ResetAttack", 1.5f);
            Invoke("ResetSpeed", 4f);
        }else {
            animator.SetBool("isWalking", true);
        }
    }

    private void PlaySound(){
        attackSound.Play();
    }

    private void ResetAttack(){
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", true);
        alreadyAttacked = false;
    }
    private void ResetSpeed(){
        agent.speed = 5f;
    }
}
