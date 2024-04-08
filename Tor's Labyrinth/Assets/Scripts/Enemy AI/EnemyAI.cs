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
    bool walkPointSet = false;
    public float walkPointRange;
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource attackSound;

    //Attacking
    public float timeBetweenAttacks;
    public static bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    private float sightRangeBase;
    public bool playerInSightRange, playerInAttackRange;
    private Vector3 enemyPosition;
    private Vector3 playerPosition;

    public AISensor sensor;

    private void Awake(){
        alreadyAttacked = false;

        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("isKilled", false);
        sensor = GetComponent<AISensor>();

        enemyPosition = transform.position;
        sightRangeBase = sightRange;
    }

    private void Update(){
        if (!CharacterControllerMovement.isSneaking && !(player.position == playerPosition)){
            sightRange = sightRangeBase * 4;
        }else {
            sightRange = sightRangeBase;
        }
        playerPosition = player.position;
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        Vector3 animFloats = Vector3.Normalize(enemyPosition - transform.position);
        animFloats = transform.InverseTransformDirection(animFloats);
        animator.SetFloat("Velocity X", -animFloats.x);
        animator.SetFloat("Velocity Z", animFloats.z);

        if (enemyPosition == transform.position){
            animator.SetBool("isWalking", false);
            walkSound.Stop();
        }else {
            if (!walkSound.isPlaying){
                walkSound.Play();
            }
            animator.SetBool("isWalking", true);
        }
        enemyPosition = transform.position;

        if (Hide.hiding && !sensor.Objects.Contains(GameObject.Find("Player"))){
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
            SearchWalkPoint();
        }

        if (walkPointSet){
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
        //ensures the AI searches a different room each walkpoint
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
            Invoke("PlaySound", 0.1f);
            Invoke("ResetAttack", timeBetweenAttacks);
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
        agent.speed = 2f;
        Invoke("ResetSpeed", 3f);
    }

    private void ResetSpeed(){
        agent.speed = 6.5f;
    }
}
