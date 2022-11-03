using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyShooterAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject pickup;

    [Header("----- Enemey Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootpos;
    [SerializeField] float shootRate;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    // Start is called before the first frame update
    void Start()
    {

    }
    #region Movement
    // Update is called once per frame
    void Update()
    {
        playerDir = (GameManager.instance.player.transform.position - transform.position);
        agent.SetDestination(GameManager.instance.player.transform.position);
        if (playerInRange)
        {  
            facePlayer();
            if (!isShooting)
                StartCoroutine(shoot());
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
           //Make object at destruction position
           //Instantiate(pickup, gameObject, transform.rotation);
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
    #region Shooting
    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootpos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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
