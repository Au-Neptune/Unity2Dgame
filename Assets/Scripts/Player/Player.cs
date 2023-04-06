using UnityEngine;
[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public ParticleSystem burst;
    public FixedJoystick joy;
    public GameUI UI;
    public bool canUseBow = false;

    private CharacterController2D controller;
    private Animator animator;
    private Rigidbody2D rig;
    private AudioSource run;
    private AudioManager audioManager;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private LayerMask enemyMask, bossMask;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float runSpeed = 40f, arrorSpeed = 600f;
    private float horizontalMove = 0f;
    private bool jump = false, Invincible = false;
    [HideInInspector] public int health = 200;

    void Awake()
	{
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        run = GetComponent<AudioSource>();
        Debug.Log(GameDatabase.operationMode);
	}

	void Update()
	{
        UI.UpdatePlayerHP(health);
        if (health <= 0) return;
        
        if (GameDatabase.operationMode == 0)
        {
            horizontalMove = Input.GetAxis("Horizontal") * runSpeed;
        }
        else if (GameDatabase.operationMode == 1)
        {
            horizontalMove = Input.GetAxis("Arrow") * runSpeed;
        }
        else if (GameDatabase.operationMode == 2)
        {
            horizontalMove = joy.Horizontal * 0.8f * runSpeed;
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (controller.isGround())  run.pitch = Mathf.Lerp(0f, 0.8f, Mathf.Abs(horizontalMove));
        else                        run.pitch = 0;

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButtonDown("Fire1") && GameDatabase.operationMode != 2)
        {
            Attack1();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Attack2();
        }
	}

    public void Jump()
    {
        jump = true;
        animator.SetBool("IsJumping", true);
    }

    public void Attack1()
    {
        animator.SetTrigger("Attack1");
    }

    public void Attack2()
    {
        if (!canUseBow) return;
        if (GameObject.Find("Bow(Clone)") != null) return;
        animator.SetTrigger("Attack2");
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void AttackSword()
    {
        audioManager.Play("sword");
        Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(attackCheck.position, 0.8f, enemyMask);
        for (int i = 0; i < enemiesToAttack.Length; i++)
        {
            Enemy e = enemiesToAttack[i].GetComponent<Enemy>();

            if (e == null)
                enemiesToAttack[i].GetComponent<Archer>().GetHit(20);
            else
                enemiesToAttack[i].GetComponent<Enemy>().GetHit(20);
        }

        Collider2D bossToAttack = Physics2D.OverlapCircle(attackCheck.position, 0.8f, bossMask);
        if (bossToAttack != null)
        {
            bossToAttack.GetComponent<Boss>().GetHit(20);
        }
    }

    public void AttackBow()
    {
        Vector3 dire;
        GameObject b = Instantiate(arrow, attackCheck.position, transform.rotation);

        if (controller.isFacingRight()) dire = Vector3.right;
        else                            dire = Vector3.left;

        b.GetComponent<Rigidbody2D>().AddForce(dire * arrorSpeed);
        audioManager.Play("bow");
        Destroy(b, 4f);
    }

    public void GetHit(int damage, Vector3 direction, float knockBack)
    {
        if (Invincible) return;

        health -= damage;
        animator.ResetTrigger("Attack1");

        if (health <= 0)
        {
            audioManager.Play("player_dead");
            animator.SetTrigger("Death");
            rig.velocity = Vector2.zero;
            run.pitch = 0;
            rig.gravityScale = 0;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            Invincible = true;
            audioManager.Play("player_hurt");
            animator.SetTrigger("GetHit");
            rig.AddForce(direction * knockBack);            
        }
    }

    public void GetHit(int damage)
    {
        if (Invincible) return;

        health -= damage;
        animator.ResetTrigger("Attack1");

        if (health <= 0)
        {
            audioManager.Play("player_dead");
            animator.SetTrigger("Death");
            rig.velocity = Vector2.zero;
            run.pitch = 0;
            rig.gravityScale = 0;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            Invincible = true;
            audioManager.Play("player_hurt");
            animator.SetTrigger("GetHit");          
        }
    }

    public void StopMoving()
    {
        animator.SetFloat("Speed", 0);
        rig.velocity = new Vector2(0, rig.velocity.y);
        run.pitch = 0;
    }

    public void InvincibleEnd()
    {
        Invincible = false;
    }

    public void ShowReturn()
    {
        UI.ShowReturnButton();
    }

    void FixedUpdate()
    {
        if (health <= 0) return;

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        animator.SetBool("Falling", controller.isFalling());

        jump = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Bow")
        {
            audioManager.Play("got_item");
            canUseBow = true;
            UI.UpdateBowState(canUseBow);
            Destroy(other.gameObject);
            burst.Play();
        }
        else if (other.name == "Healing")
        {
            health += 50;
            if (health >= 200) health = 200;
            audioManager.Play("heal");
            Destroy(other.gameObject);
            burst.Play();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, 0.8f);
    }
}
