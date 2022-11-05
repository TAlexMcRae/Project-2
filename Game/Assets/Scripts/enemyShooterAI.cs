using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyShooterAI : MonoBehaviour, InterDamage
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
        playerInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);

        playerDir = (GameManager.instance.player.transform.position - transform.position);

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
    public void inflictDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashdamage());
        if (HP <= 0)
        {
            Instantiate(pickup, shootpos.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    IEnumerator flashdamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        model.material.color = Color.white;
    }
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
