using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource audi;

    [Header("----- Stats -----")]
    // movement
    [Range(0, 10)] [SerializeField] float moveSpeed;
    [Range(0, 5)] [SerializeField] float sprintMod;
    Vector3 move;
    Vector3 playerVelo;
    float originSpeed;
    bool sprinting;

    // jumping
    [Range(0, 30)] [SerializeField] float jumpHeight;
    [Range(0, 5)] [SerializeField] int maxJumps;
    [Range(0, 100)] [SerializeField] float gravity;
    int jumpTimes;

    // health
    [Range(0, 100)] [SerializeField] public int currentHP;
    public int startHP;

    [Header("----- Shooting -----")]
    [Range(0, 1)] [SerializeField] float shootRate;
    [Range(0, 20)] [SerializeField] int shootDist;
    [Range(0, 5)] [SerializeField] int shootDMG;
    bool shooting = false;

    [Range(0, 20)][SerializeField] public int ammoCount;
    [SerializeField] GameObject hitEffect;

    [Header("----- Melee -----")]
    [Range(0, 1)][SerializeField] float meleeRate;
    [Range(0, 5)][SerializeField] int meleeDist;
    [Range(0, 5)][SerializeField] int meleeDMG;
    bool meleeAttack = false;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audiJump;
    [SerializeField] AudioClip[] audiHurt;
    [SerializeField] AudioClip[] walking;
    [SerializeField] AudioClip gunShot;
    [SerializeField] AudioClip punch;
    [SerializeField] AudioClip noAmmo;

    [Range(0, 1)] [SerializeField] float jumpVol;
    [Range(0, 1)] [SerializeField] float hurtVol;
    [Range(0, 1)] [SerializeField] float walkVol;
    [Range(0, 1)] [SerializeField] float shotVol;
    [Range(0, 1)] [SerializeField] float meleeVol;
    [Range(0, 1)] [SerializeField] float ammoVol;
    #endregion

    void Start()
    {

        originSpeed = moveSpeed;
        startHP = currentHP;
        GameManager.instance.UpdateUI();
    }

    void Update()
    {

        Movement();
        Sprint();

        StartCoroutine(Melee());
        StartCoroutine(Shoot());
    }

    void Movement()
    {

        // basic movement
        if (controller.isGrounded && playerVelo.y < 0)
        {

            jumpTimes = 0;
            playerVelo.y = 0f;
        }

        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * moveSpeed);

        // jump functions
        if (Input.GetButtonDown("Jump") && jumpTimes < maxJumps)
        {

            ++jumpTimes;
            playerVelo.y = jumpHeight;
            audi.PlayOneShot(audiJump[Random.Range(0, audiJump.Length - 1)], jumpVol);
        }

        playerVelo.y -= gravity * Time.deltaTime;
        controller.Move(playerVelo * Time.deltaTime);
    }

    void Sprint()
    {

        if (Input.GetButtonDown("Sprint"))
        {

            moveSpeed *= sprintMod;
            sprinting = true;
        }

        else if (Input.GetButtonUp("Sprint"))
        {

            moveSpeed /= sprintMod;
            sprinting = false;
        }
    }

    int powerShot;
    // shooting mechanics with raycasting
    IEnumerator Shoot()
    {
        
        if (!shooting && Input.GetButtonDown("Shoot"))
        {
            shooting = true;

            // actual shooting with gunshot noise
            if (ammoCount > 0)
            {

                RaycastHit hit;
                audi.PlayOneShot(gunShot, shotVol);

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
                {

                    if(powerShot < 5)
                    {
                        if (hit.collider.GetComponent<InterDamage>() != null)
                        {

                            hit.collider.GetComponent<InterDamage>().inflictDamage(shootDMG);
                            powerShot++;
                        }
                    }
                    else
                    {
                        if (hit.collider.GetComponent<InterDamage>() != null)
                        {

                            hit.collider.GetComponent<InterDamage>().inflictDamage(shootDMG*2);
                            powerShot = 0;
                        }
                    }


                    Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
                    ammoCount--;
                    GameManager.instance.UpdateUI();
                }
            }

            // empty gun clicking noise
            else if (ammoCount <= 0)
            {

                audi.PlayOneShot(noAmmo, ammoVol);
            }

            yield return new WaitForSeconds(shootRate);
            shooting = false;
        }
    }

    #region Damage & Respawn
    // player takes damage
    public void Damage(int dmg)
    {

        currentHP -= dmg;
        audi.PlayOneShot(audiHurt[Random.Range(0, audiHurt.Length - 1)], hurtVol);

        GameManager.instance.UpdateUI();
        StartCoroutine(GameManager.instance.PlayDMGFlash());

        if (currentHP <= 0)
        {

            GameManager.instance.deathMenu.SetActive(true);
            GameManager.instance.StartPause();
        }
    }

    // player respawn
    public void Respawn()
    {

        controller.enabled = false;
        currentHP = startHP;

        transform.position = GameManager.instance.SpawnPoint().transform.position;
        GameManager.instance.deathMenu.SetActive(false);

        GameManager.instance.deathCount++;
        GameManager.instance.UpdateUI();

        controller.enabled = true;
    }
    #endregion

    IEnumerator Melee()
    {
        meleeAttack = true;

        RaycastHit hit;
        audi.PlayOneShot(punch, meleeVol);

        if (!meleeAttack && Input.GetButtonDown("Punch"))
        {
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, meleeDist))
            {
                if (hit.collider.GetComponent<InterDamage>() != null)
                {
                    hit.collider.GetComponent<InterDamage>().inflictDamage(meleeDMG);
                }
                Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
                Damage(1);
                GameManager.instance.UpdateUI();
            }
        }
        yield return new WaitForSeconds(meleeRate);

        meleeAttack = false;
    }

    public void itemPickup(itemType itemType)
    {

        if (itemType.ammoNum != 0)
        {
            ammoCount += itemType.ammoNum;
        }

        else if (itemType.healNum != 0)
        {
            currentHP += itemType.healNum;

            if (currentHP > startHP)
            {

                currentHP = startHP;
            }

            StartCoroutine(GameManager.instance.PlayHealFlash());
        }

        GameManager.instance.UpdateUI();
    }
}