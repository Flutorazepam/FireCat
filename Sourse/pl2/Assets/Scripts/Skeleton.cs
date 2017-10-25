using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skeleton : Mob
{

    private float speed = 1.5f;
    private Vector3 direction;
    private Animator Anim;
    private SpriteRenderer sprite;
    private Character character;
    private Rigidbody2D RB;
    private Unit unit;
    private AudioClip punch;
    private AudioClip awake;
    private AudioSource audios;

    private int hp;
    private bool MoveOn;
    private float dist;

    public SkelState state
    {
        get { return (SkelState)Anim.GetInteger("state"); }
        set { Anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        direction = transform.right;
        character = FindObjectOfType<Character>();
        Anim = GetComponent<Animator>();
        unit = GetComponent<Unit>();
        punch = Resources.Load<AudioClip>("Punch");
        awake = Resources.Load<AudioClip>("Skeletrun");
        audios = GetComponent<AudioSource>();
        hp = 4;
        MoveOn = false;
    }

    void FixedUpdate()
    {
        dist = Vector2.Distance(gameObject.transform.position, character.transform.position);
        if (dist < 7 && !MoveOn) 
        {
            MoveOn = true;
            audios.PlayOneShot(awake, 0.5f);
        }
        if(MoveOn)
        {
            Move();
            if (gameObject.transform.position.x - character.transform.position.x > 0 && direction==transform.right)
            {
                direction *= -1;
                sprite.flipX = direction.x < 0.0F;
            }
            if (gameObject.transform.position.x - character.transform.position.x < 0 && direction != transform.right)
            {
                direction *= -1;
                sprite.flipX = direction.x < 0.0F;
            }
        }
    }

    private void Move()
    {
        state = SkelState.Walk;
        Vector2 overlappoint;
        overlappoint.x = gameObject.transform.position.x + direction.x;
        overlappoint.y = gameObject.transform.position.y - 0.1f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(overlappoint, 0.1f);
        if ((colliders.Length > 0) && (colliders.All(x => !x.GetComponent<Character>()) && (colliders.All(x => !x.GetComponent<Bullet>()))))
        {
            if(RB.velocity==Vector2.zero) RB.AddForce(transform.up*5, ForceMode2D.Impulse);
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    public override void ReceiveDamage()
    {
        if (unit.Directimpact == 0)
        {
            RB.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            RB.AddForce(Vector2.left * 1, ForceMode2D.Impulse);
        }
        else if (unit.Directimpact == 1)
        {
            RB.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            RB.AddForce(Vector2.right * 1, ForceMode2D.Impulse);
        }
        if (!MoveOn) MoveOn = true;
        hp--;
        if (hp < 1) Die();
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.tag == "Player") 
        {
            state = SkelState.Attack;
            audios.PlayOneShot(punch, 0.5f);
        }
    }
}
public enum SkelState
{
    Idle,
    Walk,
    Attack
}
