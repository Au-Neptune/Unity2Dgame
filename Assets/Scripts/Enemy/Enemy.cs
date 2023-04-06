using UnityEngine;

public class Enemy : MonoBehaviour
{
    private AudioManager audioManager;
    private enum State {Roaming, GetHit, Attack, Dead};
    public Transform Target;
    public int health = 40;
    public float AttackRange, walkRange, attackRadius = 0.2f, speed;
    public LayerMask WhatIsPlayer;
    private Vector3 startPosition;
    private Animator animator;
    private Rigidbody2D rigid2D;
    private Transform attackCheck;
    private Material mat;
    private State state;
    private bool facingRight;
    
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        mat = GetComponent<SpriteRenderer>().material;
        attackCheck = transform.Find("AttackCheck");
        startPosition = transform.position;
        facingRight = false;
        state = State.Roaming;
    }
    
    void Update()
    {
        if (health <= 0)
        {
            state = State.Dead;
        }
        
        if (state == State.Roaming)
        {
            Vector3 move = Vector2.left * speed * Time.deltaTime;
            transform.Translate(move);
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Walking");

            if (transform.position.x > startPosition.x + walkRange)
            {
                if (facingRight)
                    Flip();
            }
            else if (transform.position.x < startPosition.x - walkRange)
            {
                if (!facingRight)
                    Flip();
            }

            Collider2D player = Physics2D.OverlapCircle(attackCheck.position, 0.2f, WhatIsPlayer);
            if (player != null)
            {
                state = State.Attack;
            }
        }
        else if (state == State.GetHit)
        {
            animator.ResetTrigger("Attack");
        }
        else if (state == State.Attack)
        {
            animator.SetTrigger("Attack");
        }
        else if (state == State.Dead)
        {
            audioManager.Play("skeleton_death");
            Destroy(gameObject);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    public void GetHit(int damage)
    {
        health -= damage;
        state = State.GetHit;
        audioManager.Play("skeleton_hurt");
        animator.SetTrigger("GetHit");
        mat.SetInt("_Hurt", 1);
        
        if (Target.position.x > transform.position.x)
        {
            if (!facingRight)
                Flip();

            rigid2D.AddForce(Vector2.left * 500);
        }
        else
        {
            if (facingRight)
                Flip();

            rigid2D.AddForce(Vector2.right * 500);
        }
    }

    public void GetHit(int damage, Vector3 direction, float knockBack)
    {
        health -= damage;
        state = State.GetHit;
        audioManager.Play("skeleton_hurt");
        animator.SetTrigger("GetHit");
        mat.SetInt("_Hurt", 1);
    
        rigid2D.AddForce(direction * 500);
    }

    public void Attack()
    {
        Collider2D player = Physics2D.OverlapCircle(attackCheck.position, attackRadius, WhatIsPlayer);
        if (player != null)
        {
            Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);
            player.GetComponent<Player>().GetHit(20, direction, 500);  
        }
    }


    public void BackToRoam()
    {
        state = State.Roaming;
        mat.SetInt("_Hurt", 0);
    }
}
