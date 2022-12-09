using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemyShooterAI : MonoBehaviour, InterDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    public GameObject deathEffect;
    [SerializeField] GameObject[] pickup;

    [Header("----- Enemey Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int roamDist;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] GameObject headPos;
    

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootpos;
    [SerializeField] float shootRate;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    float speedOrig;
    float angleToPlayer;
    Vector3 startingPos;
    float stoppingDistanceOrg;
    // Start is called before the first frame update
    void Start()
    {
        speedOrig = agent.speed;
        startingPos = transform.position;
        stoppingDistanceOrg = agent.stoppingDistance;

        if (PlayerPref.mediumMode || PlayerPref.hardMode)
        {

            HP *= 2;
        }

        GameManager.instance.enemiesToKill++;
        GameManager.instance.UpdateUI();
        roam();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
        if (agent.enabled)
        {
            //Will find the player when all the zones are captured
            if (GameManager.instance.capturedAll)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);
                agent.speed = 10;
                
                if (playerInRange)
                {
                    canSeePlayer();
                }
            }

            else if (!GameManager.instance.capturedAll)
            {
                if (playerInRange)
                {
                    canSeePlayer();
                }
                else if (agent.remainingDistance < 0.1f && agent.destination != GameManager.instance.player.transform.position)
                    roam();
            }
        }
    }
    #region Movement + Sight
    void roam()
    {
        agent.stoppingDistance = 0;

        Vector3 randomDir = Random.insideUnitSphere * roamDist;
        randomDir += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();

        if (hit.hit)
        {
            agent.CalculatePath(hit.position, path);
        }
        agent.SetPath(path);
    }

    void canSeePlayer()
    {
        playerDir = (GameManager.instance.player.transform.position - headPos.transform.position);
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        RaycastHit Hit;
        if (Physics.Raycast(headPos.transform.position, playerDir, out Hit))
        {

            if (Hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = stoppingDistanceOrg;
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                    facePlayer();

                if (!isShooting)
                    StartCoroutine(shoot());
            }
        }
    }
    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
    }
    #endregion
    #region Damage Handling
    public void inflictDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashdamage());
        if (HP <= 0)
        {

            int chance = Random.Range(1, 3);

            if (chance == 1)
            {
                int pick = Random.Range(0, pickup.Length);
                Instantiate(pickup[pick], new Vector3(transform.position.x, 1, transform.position.z), pickup[pick].transform.rotation);
            }

            GameManager.instance.UpdateEnemies();
            GameObject temp = Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);

            Destroy(gameObject);
            Destroy(temp, 0.5f);
        }

        agent.SetDestination(GameManager.instance.player.transform.position);
    }
    IEnumerator flashdamage()
    {
        anim.SetTrigger("Damage");
        yield return new WaitForSeconds(0.3f);
    }
    #endregion
    #region Shooting
    IEnumerator shoot()
    {
        isShooting = true;
        agent.speed = 0;

        anim.SetTrigger("Attack");
        Instantiate(bullet, shootpos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        agent.speed = speedOrig;
        isShooting = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    #endregion
}
