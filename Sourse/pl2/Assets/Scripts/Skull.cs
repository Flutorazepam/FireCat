using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skull : Mob
{
    private float speed = 1.5f;
    private Vector3 direction;
    private Unit unit;
    private SpriteRenderer sprite;
    private Rigidbody2D RB;
    private Animator Anim;
    private Character character;
    private AudioSource audios;
    private int hp;
    private bool skullsound;

    public SkullState state
    {
        get { return (SkullState)Anim.GetInteger("state"); }
        set { Anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        unit = GetComponent<Unit>();
        Anim = GetComponent<Animator>();
        character = FindObjectOfType<Character>();
        audios = GetComponent<AudioSource>();
        skullsound = false;
        direction = transform.right;
        hp = 3;
    }

    void FixedUpdate ()
    {
        Move();
        if (!skullsound)
        {
            if (Vector2.Distance(gameObject.transform.position, character.transform.position) < 10)
            {
                audios.Play();
                skullsound = true;
            }
        }
    }

    private void Move()
    {
        state = SkullState.Walk;
        Vector2 overlappoint;
        overlappoint.x = gameObject.transform.position.x + direction.x;
        overlappoint.y = gameObject.transform.position.y-0.1f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(overlappoint, 0.1f);
        if ((colliders.Length > 0) && (colliders.All(x => !x.GetComponent<Character>()) && (colliders.All(x => !x.GetComponent<Bullet>()))))
        {
            direction *= -1;
            sprite.flipX = direction.x < 0.0F;
            
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
        hp--;
        if (hp < 1) Die();
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.tag == "Player")
        {
            state = SkullState.Attack;
        }
    }
}
public enum SkullState
{
    Walk,
    Attack
}