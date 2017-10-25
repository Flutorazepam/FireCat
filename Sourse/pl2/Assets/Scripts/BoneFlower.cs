using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneFlower : Mob
{
    private float CoolDown;
    private bool Attack;

    private Animator Anim;
    private Bullet bullet;
    private SpriteRenderer Sprite;
    private Character character;
    private AudioSource audios;

    public int hp = 4;
    private float dist;

    public BFState state
    {
        get { return (BFState)Anim.GetInteger("state"); }
        set { Anim.SetInteger("state", (int)value); }
    }
    void Awake ()
    {
        Anim = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
        CoolDown = 1.9f;
        Attack = false;
        character = FindObjectOfType<Character>();
        audios = GetComponent<AudioSource>();
    }
	
	void FixedUpdate ()
    {
        if(!Attack)
        {
            dist = Vector2.Distance(gameObject.transform.position, character.transform.position);
            if (dist < 8 ) Attack = true;
        }
        if (state == BFState.Cast) state = BFState.Idle;
        CoolDown -= Time.deltaTime;
        if (CoolDown <= 0 && Attack)
        {
            Cast();
        }
	}

    void Cast()
    {
        state = BFState.Cast;
        Vector3 position = transform.position;
        if (Sprite.flipX) position.x += 0.2f;
            else position.x -= 0.2f;
        Bullet NewBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        NewBullet.Parent = gameObject;
        NewBullet.Direction = NewBullet.transform.right * (Sprite.flipX ? 1 : -1);
        NewBullet.Speed = 6;
        NewBullet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Poisonball");
        CoolDown = 1.9f;
        audios.Play();
    }

    public override void ReceiveDamage()
    {
        hp--;
        if (hp < 1) Die();
        if (!Attack) Attack = true;
    }
}
public enum BFState
{
    Idle,
    Cast
}
