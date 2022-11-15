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
    [SerializeField] Animator anime;
    [SerializeField] GameObject[] pickup;

    [Header("----- Enemy Stats -----")]
    [Range(0, 20)] [SerializeField] int currentHP;
    [SerializeField] int facingSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] int aLerpSpeed;
    [SerializeField] GameObject headPos;

    Vector3 playerDirect;
    bool rangeCheck;
    float angleToPlay;

    [Header("----- Bomb -----")]
    [SerializeField] Explosion boomer;
    private bool exploding;
    #endregion

    private void Start()
    {

        GameManager.instance.enemiesToKill++;
        GameManager.instance.UpdateUI();
    }

    private void Update()
    {

        agent.SetDestination(GameManager.instance.player.transform.position);
        playerDirect = (GameManager.instance.player.transform.position - transform.position);

        if (rangeCheck)
        {

            CanSeePlayer();
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

                    agent.speed = 0;
                    agent.angularSpeed = 0;

                    StartCoroutine(Explode());
                }
            }
        }
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
            Destroy(gameObject);
        }
    }

    IEnumerator FlashDMG()
    {

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        model.material.color = Color.white;
    }

    IEnumerator Explode()
    {

        exploding = true;
        yield return new WaitForSeconds(3f);

        boomer.bombDMG *= currentHP;
        boomer.transform.position = transform.position;

        int chance = Random.Range(1, 3);

        if (chance == 1)
        {

            int pick = Random.Range(0, pickup.Length);
            Instantiate(pickup[pick], new Vector3(transform.position.x, 1, transform.position.z), pickup[pick].transform.rotation);
        }

        Destroy(gameObject);
        exploding = false;
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