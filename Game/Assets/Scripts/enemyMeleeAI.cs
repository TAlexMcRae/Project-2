using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyMeleeAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemey Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;

    [Header("----- Punch Stats -----")]
    [SerializeField] GameObject hit;
    [SerializeField] float atkSpeed;

    bool canHit;
    bool playerInRange;
    bool playerMeleeRange;
    Vector3 playerDir;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    #region Movement
    void Update()
    {
        //Only moves when within range
        if (playerInRange)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);
            playerDir = (GameManager.instance.player.transform.position - transform.position);
            facePlayer();
        }
            if (playerMeleeRange)
            {
                StartCoroutine(hitting());
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
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashdamage());
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator flashdamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        model.material.color = Color.white;
    }
    #endregion
    #region Attacking
    IEnumerator hitting()
    {
        canHit = true;
        yield return new WaitForSeconds(atkSpeed);
        canHit = false;
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
