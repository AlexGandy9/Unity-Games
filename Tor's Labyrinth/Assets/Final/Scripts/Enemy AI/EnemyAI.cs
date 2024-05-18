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
    private bool sawPlayer = false;
    public float attackSpeed;
    private float baseSpeed;

    //States
    public float sightRange, attackRange;
    private float sightRangeBase;
    public bool playerInSightRange, playerInAttackRange;
    private Vector3 enemyPosition;
    private Vector3 playerPosition;
    public AISensor sensor;
    private float timeLoop = 0f;

    //AI
    private float addFactor;
    private Vector3 initialPosition;
    private float initialDistance;

    private void Awake(){
        alreadyAttacked = false;

        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("isKilled", false);
        sensor = GetComponent<AISensor>();

        enemyPosition = transform.position;
        sightRangeBase = sightRange;

        baseSpeed = agent.speed;
        initialPosition = transform.position;
        initialDistance = Vector3.Distance(player.position, initialPosition);
    }

    private void FixedUpdate(){
        addFactor = (initialDistance - Vector3.Distance(player.position, initialPosition)) / initialDistance;
        if (addFactor > 0f){sightRangeBase += addFactor * 0.005f;}
        sightRangeBase += 0.0005f;
        print(sightRangeBase);
    }

    private void Update(){
        if (!CharacterControllerMovement.isSneaking && !(player.position == playerPosition)){
            sightRange = sightRangeBase * 3;
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

        //Animator handlers
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

        //If the enemy AI sees the player hiding, it will attack them. If not it will just walk past.
        if (Hide.hiding && sensor.enabled){
            HidingHandle();
        }else {
            sensor.enabled = true;
        }

        if (animator.GetBool("isKilled")){}
        else if (Hide.hiding){
            if (!sawPlayer){
                Patrolling();
            }else if (sawPlayer && !playerInAttackRange){
                ChasePlayer();
            }else if (sawPlayer && playerInAttackRange){
                AttackPlayer();
            }
        //If the player is hit by a trap the enemy AI will chase the player for the duration of the trap then go seeking them 
        //The AI should not chase to the last seen position as this would open up the maze for the player to find the exits easily
        }else if (HitTrap.isSlowed){
            if (playerInAttackRange){
                AttackPlayer();
            }else {
                ChasePlayer();
            }
        }else if (HitTrap2.isStuck){
            if (playerInAttackRange){
                AttackPlayer();
            }else {
                ChasePlayer();
            }
        //If none of that is true the AI will act normally and walk around the maze to find the player
        }else if (sensor.enabled){
            if (!playerInAttackRange && !playerInSightRange && !sensor.Objects.Contains(GameObject.Find("Player"))){
                Patrolling();
            }
            else if (!playerInAttackRange && (playerInSightRange || sensor.Objects.Contains(GameObject.Find("Player")))){
                ChasePlayer();
            }
            else if (playerInAttackRange && playerInSightRange){
                AttackPlayer();
            }
        //Error handler to navigate the maze
        }else {
            Patrolling();
        }
    }

    private void HidingHandle(){
        if (sensor.Objects.Contains(GameObject.Find("Player"))){
            sawPlayer = true;
        }else {
            sawPlayer = false;
        }
        sensor.enabled = false;
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }else{
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
            timeLoop = 0f;
        }
        else if (agent.velocity == Vector3.zero && timeLoop > 0.5f)
        {
            walkPointSet = false;
            timeLoop = 0f;
        }
        timeLoop += Time.deltaTime;
    }

    private void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        //ensures the AI searches a different room each walkpoint
        while (randomZ * randomZ + randomX * randomX < 120f){
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
        if(!alreadyAttacked && !animator.GetBool("isKilled")){
            animator.SetBool("isAttacking", true);
            alreadyAttacked = true;
            agent.speed = attackSpeed;
            Invoke("PlaySound", 0.1f);
            Invoke("ResetAttack", 1.5f);
            Invoke ("ResetSpeed", 4f);
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
        Invoke("ResetSpeed", 3f);
    }

    private void ResetSpeed(){
        agent.speed = baseSpeed;
    }
}
