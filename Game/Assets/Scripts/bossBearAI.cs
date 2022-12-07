using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bossBearAI : MonoBehaviour
{
    [Header("----- Boss Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] float speedChase;
    [SerializeField] float attackSpeed;
    [SerializeField] int meleeDamage;
    [SerializeField] int shockwaveDamage;
    
    

    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Object shockwave;
    [SerializeField] Object paw;
    [SerializeField] int animLerpSpeed;

    Vector3 playerDir;
    int speedOrig;
    bool playerInRange;
    bool canAttack = true;
    float stoppingDistanceOrg;
    private int attackChance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
        if (!playerInRange)
        {
            hunt();
        }
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
    void attacking()
    {
        if (playerInRange && attackChance == 1 && canAttack)
        {
            canAttack = false;
            anim.SetTrigger("Attack1");
            StartCoroutine(attackDelay());
        }
        if (playerInRange && attackChance == 2 && canAttack)
        {
            canAttack = false;
            anim.SetTrigger("Attack5");
            StartCoroutine(attackDelay());
        }
        
    }
    IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
    IEnumerator damageAnim()
    {
        yield return new WaitForSeconds(1);
    }
    public void inflictDamage(int dmg)
    {
        HP -= dmg;
        anim.SetTrigger("Get Hit Front");
        if (HP <= 0)
        {
            GameManager.instance.UpdateEnemies();
            anim.SetBool("Death", true);
            agent.enabled = false;
        }
    }
    #region Range Check
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    public void OnTriggerStay(Collider other)
    {
        attackChance = Random.Range(1, 3);
        attacking();
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
