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

    Vector3 move;
    private Vector3 playerVelocity;
    float originSpeed;
    bool sprinting;
    int jumpTimes;
    #endregion

    void Start()
    {

        originSpeed = MoveSpeed;
    }

    void Update()
    {

        Movement();
        Sprint();
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
}