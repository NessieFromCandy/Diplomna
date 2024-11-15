using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform arms;
    public Transform weaponMuzzle;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;
    public float rotationSpeed;

    //Attacking 
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public bool debug;

    private Transform player;
    private Vector3 desiredForward;
    private Vector3 transformVelocity;
    private Vector3 armsVelocity;
    private Vector3 armsDesiredForward;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        desiredForward = new Vector3(0, desiredForward.y, 0).normalized;
        var forward = Vector3.SmoothDamp(transform.forward, desiredForward, ref transformVelocity, 0.1f, rotationSpeed);
        transform.rotation = Quaternion.Euler(0, forward.y, 0);
        var armsForward = Vector3.SmoothDamp(arms.forward, armsDesiredForward, ref armsVelocity, .1f, rotationSpeed);
        arms.forward = armsForward;

        Debug.DrawRay(weaponMuzzle.position, forward * 10, Color.blue, 1f);
        Debug.DrawRay(weaponMuzzle.position, armsForward * 10, Color.yellow, 1f);

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange)
        {
            armsDesiredForward = desiredForward = (player.position - transform.position).normalized;
        }
        if (playerInAttackRange) Attacking();

    }

    private void Attacking()
    {
        if (!alreadyAttacked)
        {
            ///Attack code here 
            Rigidbody rb = Instantiate(projectile, weaponMuzzle.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(arms.forward * 32f, ForceMode.Impulse);
            ///
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

