using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, InterDamage
{

    #region Variables
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource audi;

    [Header("----- Stats -----")]
    // movement
    [Range(0, 10)] [SerializeField] float moveSpeed;
    [Range(0, 5)] [SerializeField] float sprintMod;
    [SerializeField] int pushBackTime;

    Vector3 move;
    Vector3 playerVelo;
    float originSpeed;
    bool sprinting;
    public Vector3 pushBack;

    // jumping
    [Range(0, 30)] [SerializeField] float jumpHeight;
    [Range(0, 5)] [SerializeField] int maxJumps;
    [Range(0, 100)] [SerializeField] float gravity;
    int jumpTimes;

    // health
    [Range(0, 100)] [SerializeField] public int currentHP;
    public int startHP;
    public int playerLives;
    private bool dead;

    [Header("----- Shooting -----")]
    [Range(0, 1)] [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [Range(0, 5)] [SerializeField] int shootDMG;
    [Range(0, 20)] [SerializeField] public int ammoCount;
    [SerializeField] GameObject hitEffect;

    private int startDMG;
    int powerShot;

    // boosting
    bool shooting = false;
    bool boost = false;
    public float boostTime = 10.2f;

    private GunRecoil recoilScript;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;
    bool throwReady;
    public GameObject grenade;
    public int grenadeCounter;
    public float throwCooldown;
    public Transform attackPoint;

    [Header("----- Melee -----")]
    [Range(0, 1)][SerializeField] float meleeRate;
    [Range(0, 5)][SerializeField] int meleeDist;
    [Range(0, 5)][SerializeField] int meleeDMG;
    bool meleeAttack = false;
    private int startMelee;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audiJump;
    [SerializeField] AudioClip[] audiHurt;
    [SerializeField] AudioClip[] walking;
    [SerializeField] AudioClip gunShot;
    [SerializeField] AudioClip noAmmo;
    [SerializeField] AudioClip ammoPickUpSFX;
    [SerializeField] AudioClip HPPickUpSFX;
    [SerializeField] AudioClip dmgPickUpSFX;
    [SerializeField] AudioClip[] meleeSFX;
    [SerializeField] AudioClip deathSFX;

    [Range(0, 1)] [SerializeField] float jumpVol;
    [Range(0, 1)] [SerializeField] float hurtVol;
    [Range(0, 1)] [SerializeField] float walkVol;
    [Range(0, 1)] [SerializeField] float shotVol;
    [Range(0, 1)] [SerializeField] float meleeVol;
    [Range(0, 1)] [SerializeField] float ammoVol;

    [Header("----- Animation -----")]
    [SerializeField] Animator animator;
    #endregion

    void Start()
    {

        originSpeed = moveSpeed;
        startHP = currentHP;
        GameManager.instance.UpdateUI();
        
        
        startDMG = shootDMG;
        startMelee = meleeDMG;
        sprinting = false;
        throwReady = true;
        dead = false;

        recoilScript = transform.Find("CamRotate/CamRecoil").GetComponent<GunRecoil>();

        if (PlayerPref.mediumMode == true || PlayerPref.hardMode == true)
        {

            playerLives = 5;
        }

        else { playerLives = 3; }

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackTime);

        Movement();
        Sprint();

        StartCoroutine(Melee());
        StartCoroutine(Shoot());

        if (Input.GetButtonDown("Grenade") && throwReady && grenadeCounter > 0)
        {
            Throw();
        }

        //Walking animation
        //bool isWalking = animator.GetBool("isWalking");
        bool forwardPressed = Input.GetAxis("Vertical") != 0;
        if (/*isWalking &&*/ forwardPressed)
        {
            animator.SetBool("isWalking", true);
        }
        else //if (/*isWalking &&*/ !forwardPressed)
        {
            animator.SetBool("isWalking", false);
        }

    }

    #region Movement
    void Movement()
    {

        // basic movement
        if (controller.isGrounded && playerVelo.y < 0)
        {

            jumpTimes = 0;
            playerVelo.y = 0f;
        }

        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move((move + pushBack) * Time.deltaTime * moveSpeed);

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
    #endregion

    #region Grenade Throwing
    void Throw()
    {
        throwReady = false;
        GameObject projectile = Instantiate(grenade, attackPoint.position, Camera.main.transform.rotation);
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = Camera.main.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        grenadeCounter--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        throwReady = true;
    }
    #endregion

    #region Damage Dealing
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

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
                {

                    audi.PlayOneShot(gunShot, shotVol);

                    if (powerShot < 5)
                    {

                        if (hit.collider.GetComponent<InterDamage>() != null)
                        {

                            hit.collider.GetComponent<InterDamage>().inflictDamage(shootDMG);
                            powerShot++;
                        }
                    }

                    else if (powerShot == 5)
                    {

                        if (hit.collider.GetComponent<InterDamage>() != null)
                        {

                            hit.collider.GetComponent<InterDamage>().inflictDamage(shootDMG * 2);
                            powerShot = 0;
                        }
                    }
                }

                Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
                recoilScript.RecoilFire();

                ammoCount--;
                GameManager.instance.UpdateUI();
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

    IEnumerator Melee()
    {

        if (!meleeAttack && Input.GetButtonDown("Punch"))
        {
            meleeAttack = true;

            RaycastHit hit;
            audi.PlayOneShot(meleeSFX[Random.Range(0, audiJump.Length - 1)], meleeVol);

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, meleeDist))
            {
                if (hit.collider.GetComponent<InterDamage>() != null)
                {
                    hit.collider.GetComponent<InterDamage>().inflictDamage(meleeDMG);
                }
                Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
                inflictDamage(1);
                GameManager.instance.UpdateUI();
            }
        }
        yield return new WaitForSeconds(meleeRate);

        meleeAttack = false;
    }
    #endregion

    #region Damage & Respawn
    // player takes damage
    public void inflictDamage(int dmg)
    {

        currentHP -= dmg;
        audi.PlayOneShot(audiHurt[Random.Range(0, audiHurt.Length - 1)], hurtVol);

        GameManager.instance.UpdateUI();
        StartCoroutine(GameManager.instance.PlayDMGFlash());

        if (currentHP <= 0 && !dead)
        {

            if (boost)
            {

                StopCoroutine(Boost(0, 0f));
                GameManager.instance.playBoostScreen.SetActive(false);
                GameManager.instance.boostTimer.SetActive(false);
            }

            dead = true;
            playerLives--;

            if (playerLives > 0)
            {
                audi.PlayOneShot(deathSFX);
                GameManager.instance.deathMenu.SetActive(true);
                GameManager.instance.StartPause();
            }

            else if (playerLives < 0)
            {
                audi.PlayOneShot(deathSFX);
                GameManager.instance.gameOverMenu.SetActive(true);
                GameManager.instance.StartPause();
            }
        }
    }

    // player respawn
    public void Respawn()
    {

        GameManager.instance.deathMenu.SetActive(false);
        controller.enabled = false;
        currentHP = startHP;

        transform.position = GameManager.instance.SpawnPoint().transform.position;
        dead = false;

        if (ammoCount < 20) { ammoCount = 20; }
        GameManager.instance.UpdateUI();

        controller.enabled = true;
    }
    #endregion

    #region Pickups
    public void itemPickup(itemType itemType)
    {

        if (itemType.ammoNum != 0)
        {
            audi.PlayOneShot(ammoPickUpSFX);
            ammoCount += itemType.ammoNum;

            grenadeCounter += Random.Range(0, 2);
        }

        else if (itemType.healNum != 0)
        {
            audi.PlayOneShot(HPPickUpSFX);
            currentHP += itemType.healNum;

            if (currentHP > startHP)
            {

                currentHP = startHP;
            }

            StartCoroutine(GameManager.instance.PlayHealFlash());
        }

        else if (itemType.boostPow != 0 && !boost)
        {
            audi.PlayOneShot(dmgPickUpSFX);
            StartCoroutine(Boost(itemType.boostPow, boostTime));
        }

        GameManager.instance.UpdateUI();
    }

    IEnumerator Boost(int power, float timer)
    {

        boost = true;
        shootDMG *= power;
        meleeDMG *= power;

        GameManager.instance.playBoostScreen.SetActive(true);
        GameManager.instance.boostTimer.SetActive(true);
        GameManager.instance.boostSeconds = timer;

        yield return new WaitForSeconds(10f);

        GameManager.instance.playBoostScreen.SetActive(false);
        GameManager.instance.boostTimer.SetActive(false);

        shootDMG = startDMG;
        meleeDMG = startMelee;
        boost = false;
    }
    #endregion
}