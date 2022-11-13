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
    [SerializeField] GameObject pickup;

    [Header("----- Enemy Stats -----")]
    [Range(0, 20)] [SerializeField] int currentHP;
    [SerializeField] int facingSpeed;

    [Header("----- Bomb -----")]
    [Range(0, 5)] [SerializeField] int bombDMG;
    bool exploding;
    Vector3 playerDirect;
    bool rangeCheck;
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

            FacePlayer();

            if (!exploding)
            {

                StartCoroutine(Explode());
            }
        }
    }

    private void FacePlayer()
    {

        playerDirect.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDirect);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * facingSpeed);
    }

    public void inflictDamage(int dmg)
    {

        currentHP -= dmg;
        StartCoroutine(FlashDMG());
        if (currentHP <= 0)
        {

            Instantiate(pickup, transform.position, transform.rotation);
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

        // stops the bomb enemy
        agent.speed = 0;
        agent.angularSpeed = 0;
        agent.acceleration = 0;

        yield return new WaitForSeconds(3f);
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