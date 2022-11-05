using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

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
    [Range(0, 100)] [SerializeField] int currentHP;
    int startHP;

    [Header("----- Shooting -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDMG;
    bool shooting = false;
    #endregion

    void Start()
    {

        originSpeed = moveSpeed;
        startHP = currentHP;
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

        if (shooting == false && Input.GetButtonDown("Shoot"))
        {

            shooting = true;
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {

                if (hit.collider.GetComponent<InterDamage>() != null)
                {

                    hit.collider.GetComponent<InterDamage>().inflictDamage(shootDMG);
                }
            }

            yield return new WaitForSeconds(shootRate);
            shooting = false;
        }
    }

    // player damage
    public void Damage(int dmg)
    {

        currentHP -= dmg;
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

        transform.position = GameManager.instance.spawnPos.transform.position;
        GameManager.instance.deathMenu.SetActive(false);

        controller.enabled = true;
    }
}