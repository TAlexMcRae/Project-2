using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class rockMonsterAI : MonoBehaviour, InterDamage
{

    #region Variables
    [Header("---- Components ----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    public GameObject deathEffect;
    [SerializeField] GameObject[] pickup;
    [SerializeField] AudioSource audi;

    [Header("---- Enemy ----")]
    [SerializeField] int currentHP;
    [SerializeField] int facingSpeed;
    [SerializeField] int roamDist;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] GameObject headPos;

    private Vector3 playDirect;
    private Vector3 startPos;
    private bool rangeCheck;
    private float speedOrig;
    private float angle2Play;
    private float stopOrig;
    private Color origCol;

    [Header("---- Shooting ----")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;
    [SerializeField] AudioClip shootSFX;

    private bool shooting;

    [Header("---- Bomb ----")]
    [SerializeField] GameObject boomer;
    [SerializeField] AudioClip boomSFX;
    #endregion

    private void Start()
    {

        speedOrig = agent.speed;
        startPos = transform.position;
        stopOrig = agent.stoppingDistance;
        origCol = model.material.color;

        if (PlayerPref.mediumMode || PlayerPref.hardMode)
        {

            currentHP *= 2;
        }

        GameManager.instance.enemiesToKill++;
        GameManager.instance.UpdateUI();
        Roaming();
    }

    private void Update()
    {

        anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

        if (agent.enabled)
        {

            // if all zonees have been captured this wave, chase player
            if (GameManager.instance.capturedAll)
            {

                agent.SetDestination(GameManager.instance.player.transform.position);
                agent.speed = 7;

                if (rangeCheck)
                {

                    CanSeePlayer();
                }
            }

            else if (!GameManager.instance.capturedAll)
            {

                if (rangeCheck)
                {

                    CanSeePlayer();
                }

                else if (agent.remainingDistance < 0.1f && agent.destination != GameManager.instance.player.transform.position)
                {

                    Roaming();
                }
            }
        }
    }

    #region Movement
    private void Roaming()
    {

        agent.stoppingDistance = 0;

        Vector3 randDir = Random.insideUnitSphere * roamDist;
        randDir += startPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randDir, out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();

        if (hit.hit)
        {

            agent.CalculatePath(hit.position, path);
        }

        agent.SetPath(path);
    }

    private void CanSeePlayer()
    {

        playDirect = (GameManager.instance.player.transform.position - headPos.transform.position);
        angle2Play = Vector3.Angle(playDirect, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(headPos.transform.position, playDirect, out hit))
        {

            if (hit.collider.CompareTag("Player") && angle2Play <= sightAngle)
            {

                agent.stoppingDistance = 5;
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {

                    FacePlayer();
                }

                else if (!shooting)
                {

                    StartCoroutine(Shoot());
                }
            }
        }
    }

    private void FacePlayer()
    {

        playDirect.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playDirect);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * facingSpeed);
    }
    #endregion

    #region Health & Damage
    public void inflictDamage(int dmg)
    {

        currentHP -= dmg;
        StartCoroutine(FlashDMG());

        if (currentHP <= 0)
        {

            int chance = Random.Range(1, 3);

            if (chance == 1)
            {

                int pick = Random.Range(0, pickup.Length);
                Instantiate(pickup[pick], new Vector3(transform.position.x, 1, transform.position.z), pickup[pick].transform.rotation);
            }

            GameManager.instance.UpdateEnemies();
            GameObject temp1 = Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
            GameObject temp2 = Instantiate(boomer, transform.position, deathEffect.transform.rotation);
            audi.PlayOneShot(boomSFX);

            Destroy(gameObject);
            Destroy(temp1, 0.5f);
            Destroy(temp2, 0.5f);
        }

        else { agent.SetDestination(GameManager.instance.player.transform.position); }
    }

    private IEnumerator FlashDMG()
    {

        anim.SetTrigger("Damaged");
        model.material.color = Color.black;
        yield return new WaitForSeconds(0.3f);
        model.material.color = origCol;
        yield return new WaitForSeconds(0.3f);
    }

    private IEnumerator Shoot()
    {

        shooting = true;
        agent.speed = 0;
        
        anim.SetTrigger("Attack");
        audi.PlayOneShot(shootSFX);
        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        agent.speed = speedOrig;
        shooting = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rangeCheck = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rangeCheck = false;
            Roaming();
        }
    }
    #endregion
}