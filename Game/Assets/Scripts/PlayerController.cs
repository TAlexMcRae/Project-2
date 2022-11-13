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

    [Range(0, 20)] [SerializeField] public int ammoCount;
    [SerializeField] GameObject hitEffect;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audiJump;
    [SerializeField] AudioClip[] audiHurt;
    [SerializeField] AudioClip walking;

    [Range(0, 1)] [SerializeField] float jumpVol;
    [Range(0, 1)] [SerializeField] float hurtVol;
    [Range(0, 1)] [SerializeField] float walkVol;
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

    // shooting mechanics with raycasting
    IEnumerator Shoot()
    {

        if (!shooting && Input.GetButtonDown("Shoot"))
        {

            if (ammoCount > 0)
            {

                shooting = true;
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
                {

                    if (hit.collider.GetComponent<InterDamage>() != null)
                    {

                        hit.collider.GetComponent<InterDamage>().inflictDamage(shootDMG);
                        ammoCount--;
                        GameManager.instance.UpdateUI();
                    }

                    Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
                }

                yield return new WaitForSeconds(shootRate);
                shooting = false;
            }
        }
    }

    #region Damage & Respawn
    // player takes damage
    public void Damage(int dmg)
    {

        currentHP -= dmg;

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