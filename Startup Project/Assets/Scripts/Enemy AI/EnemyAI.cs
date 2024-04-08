using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public static Animator animator;
    private NavMeshAgent agent;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    public static bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    private float sightRangeBase;
    public bool playerInSightRange, playerInAttackRange;
    private Vector3 enemyPosition;

    public AISensor sensor;

    private void Awake(){
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        sensor = GetComponent<AISensor>();

        enemyPosition = transform.position;
        sightRangeBase = sightRange;
    }

    private void Update(){
        if (!CharacterControllerMovement.isSneaking && player.velocity> 0f){
            sightRange = sightRangeBase * 3;
        }else {
            sightRange = sightRangeBase;
        }
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if (enemyPosition == transform.position){
            animator.SetBool("isWalking", false);
        }else {
            animator.SetBool("isWalking", true);
        }
        enemyPosition = transform.position;

        if (Hide.hiding){
            Patrolling();
        }else if (animator.GetBool("isKilled")){
        }else {
            if (!playerInAttackRange && !playerInSightRange && !sensor.Objects.Contains(GameObject.Find("Player"))) Patrolling();
            if (!playerInAttackRange && (playerInSightRange || sensor.Objects.Contains(GameObject.Find("Player")))) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
    }

    private void Patrolling(){
        if (!walkPointSet){
            //animator.SetBool("isWalking", false);
            SearchWalkPoint();
        }

        if (walkPointSet){
            //animator.SetBool("isWalking", true);
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
        else if (agent.velocity == Vector3.zero)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        while (randomZ * randomZ + randomX * randomX < 100f){
            randomZ = Random.Range(-walkPointRange, walkPointRange);
            randomX = Random.Range(-walkPointRange, walkPointRange);
        }

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void ChasePlayer(){
        agent.SetDestination(player.position);
        LookAtPlayer();
    }

    private void LookAtPlayer(){
        //No y position so enemy doesnt rotate up and down.
        Vector3 relativePos = new Vector3(player.position.x, 0, player.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * 30);
    }

    private void AttackPlayer(){
        LookAtPlayer();
        agent.SetDestination(transform.position);
        if(!alreadyAttacked){
            animator.SetBool("isAttacking", true);
            alreadyAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }else {
            animator.SetBool("isWalking", true);
        }
    }

    private void ResetAttack(){
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", true);
        alreadyAttacked = false;
        agent.speed = 2f;
        Invoke("ResetSpeed", 4f);
    }

    private void ResetSpeed(){
        agent.speed = 6.5f;
    }
}
