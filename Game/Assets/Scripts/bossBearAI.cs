using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bossBearAI : MonoBehaviour
{
    [Header("----- Boss Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int speedChase;
    [SerializeField] int animLerpSpeed;
    [SerializeField] GameObject headPos;
    [SerializeField] int meleeDamage;
    [SerializeField] int shockwaveDamage;

    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] GameObject deathEffect;

    Vector3 playerDir;
    int speedOrig;
    bool playerInRange;
    float stoppingDistanceOrg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hunt();
    }
    void hunt()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
    }
    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
    }
    void meleeAttack()
    {
        if (playerInRange)
        {

        }
    }
    void jumpShockwave()
    {

        //GameManager.instance.playerScript.pushBack = ((objects[rnr].transform.position - transform.position).normalized) * forceAMT;
    }
    #region Range Check
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
