using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyBomberAI : MonoBehaviour, InterDamage
{

    #region Variables
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    public GameObject deathEffect;
    [SerializeField] GameObject[] pickup;
    [SerializeField] GameObject explosionEffect;


    [Header("----- Enemy Stats -----")]
    [Range(0, 20)] [SerializeField] int currentHP;
    [SerializeField] int facingSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] int aLerpSpeed;
    [SerializeField] int roamDist;
    [SerializeField] GameObject headPos;
    

    Vector3 playerDirect;
    Vector3 startPos;
    bool rangeCheck;
    float angleToPlay;

    [Header("----- Bomb -----")]
    public GameObject boomer;
    private bool exploding;
    private bool boom;
    #endregion

    private void Start()
    {

        startPos = transform.position;
        GameManager.instance.enemiesToKill++;
        GameManager.instance.UpdateUI();
        boom = false;
        Roaming();
    }

    private void Update()
    {
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * aLerpSpeed));
        if (agent.enabled)
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

        if (exploding && !boom)
        {

            StartCoroutine(Boom());
        }
    }

    private void FacePlayer()
    {

        playerDirect.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDirect);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * facingSpeed);
    }

    private void CanSeePlayer()
    {

        playerDirect = (GameManager.instance.player.transform.position - headPos.transform.position);
        angleToPlay = Vector3.Angle(playerDirect, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(headPos.transform.position, playerDirect, out hit))
        {

            if (hit.collider.CompareTag("Player") && angleToPlay <= sightAngle)
            {

                agent.stoppingDistance = 5;
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {

                    FacePlayer();
                }

                else if (!exploding)
                {

                    StartCoroutine(Explode());
                }
            }
        }
    }

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
            Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);

            Destroy(gameObject);
        }
    }

    IEnumerator FlashDMG()
    {
        anim.SetTrigger("Damage");
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator Boom()
    {

        boom = true;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        model.material.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        boom = false;
    }

    IEnumerator Explode()
    {

        exploding = true;
        yield return new WaitForSeconds(3f);

        Instantiate(boomer, gameObject.transform.position, gameObject.transform.rotation);
        Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        int chance = Random.Range(1, 3);

        if (chance == 1)
        {

            int pick = Random.Range(0, pickup.Length);
            Instantiate(pickup[pick], new Vector3(transform.position.x, 1, transform.position.z), pickup[pick].transform.rotation);
        }

        Destroy(gameObject);
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
        }
    }
}