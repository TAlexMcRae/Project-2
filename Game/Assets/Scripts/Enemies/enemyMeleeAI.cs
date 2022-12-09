using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyMeleeAI : MonoBehaviour, InterDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject pickup;

    [Header("----- Enemey Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;

    [Header("----- Punch Stats -----")]
    [SerializeField] Transform hit;
    [SerializeField] float atkSpeed;

    bool canHit;
    bool playerInRange;
    bool playerMeleeRange;
    Vector3 playerDir;
    // Start is called before the first frame update
    void Start()
    {

        GameManager.instance.enemiesToKill++;
        GameManager.instance.UpdateUI();
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
    }

    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
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
    #region Enemy Damage Handling
    public void inflictDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashdamage());
        if (HP <= 0)
        {
            Instantiate(pickup, hit.position, transform.rotation);
            GameManager.instance.UpdateEnemies();
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

}
