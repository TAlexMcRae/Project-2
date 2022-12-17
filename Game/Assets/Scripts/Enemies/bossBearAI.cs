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
    [SerializeField] public int meleeDamage;
    
    

    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform paw;
    [SerializeField] int animLerpSpeed;
    [SerializeField] AudioSource audi;
    [SerializeField] AudioSource bossBGM;
    [SerializeField] AudioClip claws;
    [SerializeField] PlayerController playerDmg;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] GameObject player;

    Vector3 playerDir;
    int speedOrig;
    private bool playerInRange = false;
    [SerializeField] bool canAttack = true;
    float stoppingDistanceOrg;
    private int attackChance;
    [SerializeField] float attackTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(bossBGM);
        if (PlayerPref.mediumMode)
        {

            HP *= 2;
        }

        else if (PlayerPref.hardMode)
        {

            HP *= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
        //playerInRange = false;
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack5", false);
        if (Vector3.Distance(player.transform.position, transform.position) <= sphereCollider.radius * transform.localScale.x)
        {
            //playerInRange = true;
            attackChance = Random.Range(1, 3);
            attacking();
        }
        else
        {
            canAttack = false;
            hunt();
        }
        attackDelay();
    }
    #region Movement
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
    #endregion
    #region Attacking
    void attacking()
    {
        if (/*playerInRange &&*/ attackChance == 1 && canAttack)
        {
            canAttack = false;
            anim.SetBool("Attack1", true);
            audi.PlayOneShot(claws);
            attackTimer = 0;
        }
        if (/*playerInRange &&*/ attackChance == 2 && canAttack)
        {
            canAttack = false;
            anim.SetBool("Attack5", true);
            audi.PlayOneShot(claws);
            attackTimer = 0;
        }
    }
    
    void attackDelay()
    {
        //yield return new WaitForSeconds(attackSpeed);
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackSpeed)
        canAttack = true;
    }
    IEnumerator damageAnim()
    {
        yield return new WaitForSeconds(1);
    }
    public void attackDamage()
    {
        GameManager.instance.playerScript.GetComponent<InterDamage>().inflictDamage(meleeDamage);
    }
    /*public void swDamage()
    {
        GameManager.instance.playerScript.GetComponent<InterDamage>().inflictDamage(shockwaveDamage);
    }*/
    #endregion
    #region Damage Handling
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
    #endregion
    #region Range Check
    /*public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attackTimer = 5;
            //playerInRange = true;
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attackChance = Random.Range(1, 3);
            attacking();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.)
        {
            playerInRange = false;
        }
    }*/
    #endregion
}
