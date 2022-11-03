using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BomberAI : MonoBehaviour, InterDamage
{

    #region Variables
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Character Stats -----")]
    [Range(0, 20)] [SerializeField] int HP;
    [Range(0, 5)] [SerializeField] int facingSpeed;

    [Header("----- Bomb Stats -----")]
    [Range(0, 5)] [SerializeField] int bombDMG;
    [Range(0, 5)] [SerializeField] float boomTime;

    bool exploding;
    bool rangeCheck;
    Vector3 playerDirect;
    #endregion

    void Start()
    {
        //GameManager.instance.EnemiesToKill++;
    }

    void Update()
    {

        agent.SetDestination(GameManager.instance.player.transform.position);
        playerDirect = GameManager.instance.player.transform.position - transform.position;

        if (rangeCheck)
        {

            FacePlayer();

            if (!exploding)
            {

                StartCoroutine(Explosion());
            }
        }
    }

    void FacePlayer()
    {

        playerDirect.y = 0;

        Quaternion rotation = Quaternion.LookRotation(playerDirect);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * facingSpeed);
    }

    public void inflictDamage(int dmg)
    {

        HP -= dmg;

        StartCoroutine(FlashDMG());

        if (HP <= 0)
        {
            //GameManager.instance.UpdateEnemies();
            Destroy(gameObject);
        }
    }

    IEnumerator FlashDMG()
    {

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        model.material.color = Color.white;
    }

    IEnumerator Explosion()
    {

        exploding = true;

        yield return new WaitForSeconds(boomTime);
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