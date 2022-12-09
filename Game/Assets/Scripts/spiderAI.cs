using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class spiderAI : MonoBehaviour, InterDamage
{

    #region Variables
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    public GameObject deathEffect;
    [SerializeField] GameObject[] pickup;

    [Header("----- Stats -----")]
    [Range(0, 20)] [SerializeField] int currentHP;
    [SerializeField] int facingSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] GameObject headPos;

    Vector3 playerDirect;
    float angle2Player;

    [Header("----- Melee Stats -----")]
    [SerializeField] int hitDamage;
    [SerializeField] float hitSpeed;

    bool rangeCheck;
    bool attacking;
    #endregion

    private void Start()
    {

        if (PlayerPref.mediumMode)
        {

            currentHP *= 2;
            hitDamage *= 2;
        }

        else if (PlayerPref.hardMode)
        {

            currentHP *= 2;
            hitDamage *= 4;
        }

        GameManager.instance.enemiesToKill++;
        GameManager.instance.UpdateUI();
    }

    private void Update()
    {

        if (agent.enabled)
        {

            agent.SetDestination(GameManager.instance.player.transform.position);
            CanSeePlayer();

            if (rangeCheck && !attacking)
            {

                StartCoroutine(Attack());
            }
        }
    }

    #region Movement
    private void CanSeePlayer()
    {

        playerDirect = (GameManager.instance.player.transform.position - headPos.transform.position);
        angle2Player = Vector3.Angle(playerDirect, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(headPos.transform.position, playerDirect, out hit))
        {

            if (hit.collider.CompareTag("Player") && angle2Player <= sightAngle)
            {

                agent.stoppingDistance = 5;
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {

                    FacePlayer();
                }
            }
        }
    }

    private void FacePlayer()
    {

        playerDirect.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDirect);
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
            GameObject temp = Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);

            Destroy(gameObject);
            Destroy(temp, 0.5f);
        }
    }

    IEnumerator FlashDMG()
    {

        yield return new WaitForSeconds(0.3f);
    }
    #endregion

    #region Melee Action
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

    private IEnumerator Attack()
    {

        attacking = true;
        GameManager.instance.playerScript.GetComponent<InterDamage>().inflictDamage(hitDamage);

        yield return new WaitForSeconds(hitSpeed);
        attacking = false;
    }
    #endregion
}