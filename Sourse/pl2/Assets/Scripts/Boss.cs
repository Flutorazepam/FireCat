using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Mob
{
    private int hp;
    public int Hp { get { return hp; } set { hp = value; } }

    private float speed;
    private float CoolDown;
    private bool Attack;
    private Vector3 position;
    private Character character;
    private Bullet bullet;
    private BossHpBar Hpbar;
    private AudioSource audios;
    private AudioClip eyeshot;
    private AudioClip awake;

    private void Awake()
    {
        character = FindObjectOfType<Character>();
        bullet = Resources.Load<Bullet>("Bullet");
        Hpbar = FindObjectOfType<BossHpBar>();
        audios = GetComponent<AudioSource>();
        eyeshot = Resources.Load<AudioClip>("Eyeshot");
        awake = Resources.Load<AudioClip>("Bossawake");
        speed = 2.5f;
        hp = 10;
        position = transform.position;
        Attack = false;
    }

    void FixedUpdate ()
    {
        if (!Attack)
        {
            if (Vector2.Distance(gameObject.transform.position, character.transform.position) < 16)
            {
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
                GameObject.Find("Bosstheme").GetComponent<AudioSource>().Play();
                audios.PlayOneShot(awake, 1);
                Attack = true;
                Hpbar.Refresh();
            }
        }

        CoolDown -= Time.deltaTime;
        if (CoolDown <= 0 && Attack)
        {
            Cast();
        }

        Move();
	}

    void Move()
    {
        if (transform.position.y == -16) position.y = -8;
        else if (transform.position.y == -8) position.y = -16;
        transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
    }

    void Cast()
    {
        Bullet NewBullet = Instantiate(bullet, transform.position, bullet.transform.rotation) as Bullet;
        NewBullet.Parent = gameObject;
        if (character.transform.position.x < 79) 
        {
            NewBullet.Direction = NewBullet.transform.right * -1;
            NewBullet.GetComponent<SpriteRenderer>().flipX = -NewBullet.Direction.x < 0.0F;
        }
        else 
        {
            NewBullet.Direction = NewBullet.transform.right;
            NewBullet.GetComponent<SpriteRenderer>().flipX = -NewBullet.Direction.x < 0.0F;
        }
        NewBullet.Speed = 15;
        NewBullet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("eye");
        CoolDown = 0.6f;
        audios.PlayOneShot(eyeshot, 0.2f);
    }

    public override void ReceiveDamage()
    {
        Hp--;
        Hpbar.Refresh();
        if (Hp < 1)
        {
            GameObject.Find("Bosstheme").GetComponent<AudioSource>().Stop();
            GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
            GameObject.FindGameObjectWithTag("Stop").SetActive(false);
            Die();
        }
    }
}
