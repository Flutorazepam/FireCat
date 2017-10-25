using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fly : Mob{

    private float speed = 2;
    private Vector3 direction;
    private SpriteRenderer sprite;
    private Character character;
    private GameObject Explosion;
    private AudioSource audios;

    private bool MoveOn;
    private float dist;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        Explosion = Resources.Load<GameObject>("Explosion");
        character = FindObjectOfType<Character>();
        audios = GetComponent<AudioSource>();
        direction = transform.right;
        MoveOn = false;
    }

    void FixedUpdate()
    {
        if(!MoveOn) 
        {
            dist = Vector2.Distance(gameObject.transform.position, character.transform.position);
            if (dist < 10 && !MoveOn)
            {
                MoveOn = true;
                audios.Play();
            }
        }
        if (MoveOn)
        {
            Move();
            if (gameObject.transform.position.x - character.transform.position.x > 0 && direction == transform.right)
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
        transform.position = Vector3.MoveTowards(transform.position, character.transform.position, speed * Time.deltaTime);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Die();
        Instantiate(Explosion, transform.position, transform.rotation);
    }

    public override void ReceiveDamage()
    {
        Die();
        Instantiate(Explosion, transform.position, transform.rotation);
    }

}
