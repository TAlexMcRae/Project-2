using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables
    [Header("----- Components -----")]
    [SerializeField] CharacterController Controller;

    [Header("----- Player -----")]
    // movement
    [Range(0, 10)] [SerializeField] float MoveSpeed;
    [Range(0, 5)] [SerializeField] float SprintMod;

    // jumping
    [Range(0, 30)] [SerializeField] float JumpHeight;
    [Range(0, 5)] [SerializeField] int MaxJumps;
    [Range(0, 100)] [SerializeField] float Gravity;

    // health
    [Range(0, 10)] [SerializeField] int HP;

    [Header("----- Gun Stats pew pew!! -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDMG;

    bool isShooting;

    Vector3 move;
    private Vector3 playerVelocity;
    float originSpeed;
    bool sprinting;
    int jumpTimes;
    int originHP;
    #endregion

    void Start()
    {

        originSpeed = MoveSpeed;
        originHP = HP;
    }

    void Update()
    {

        Movement();
        Sprint();

        if (!isShooting)
            StartCoroutine(shoot());
    }

    // basic moving functions
    void Movement()
    {

        if (Controller.isGrounded && playerVelocity.y < 0)
        {

            jumpTimes = 0;
            playerVelocity.y = 0f;
        }

        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        Controller.Move(move * Time.deltaTime * MoveSpeed);

        if (Input.GetButtonDown("Jump") && jumpTimes < MaxJumps)
        {

            ++jumpTimes;
            playerVelocity.y = JumpHeight;
        }

        playerVelocity.y -= Gravity * Time.deltaTime;
        Controller.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint()
    {

        if (Input.GetButtonDown("Sprint"))
        {

            MoveSpeed *= SprintMod;
            sprinting = true;
        }

        else if (Input.GetButtonUp("Sprint"))
        {

            MoveSpeed /= SprintMod;
            sprinting = false;
        }
    }

    public void Respawn()
    {

        Controller.enabled = false;
        HP = originHP;

        transform.position = GameManager.instance.spawnPos.transform.position;
        GameManager.instance.deathMenu.SetActive(false);
        
        Controller.enabled = true;
    }

    public void Damage(int dmg)
    {

        HP -= dmg;
        StartCoroutine(GameManager.instance.PlayDMGFlash());

        if (HP <= 0)
        {

            GameManager.instance.deathMenu.SetActive(true);
            GameManager.instance.StartPause();
        }
    }

    IEnumerator shoot()
    {
        if (!isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {

                if (hit.collider.GetComponent<InterDamage>() != null)
                {

                    hit.collider.GetComponent<InterDamage>().inflictDamage(shootDMG);
                }
            }
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }
}