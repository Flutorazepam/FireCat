using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character : Unit
{
    private int hp;
    public int Hp
    {
        get { return hp; }
        set { if (value < 9) hp = value; }
    }

    private float WalkSpeed = 3;
    private float JumpForce = 5;
    private float CoolDown = 0;
    private float WalkCD;

    private Rigidbody2D RB;
    private SpriteRenderer Sprite;
    private Animator Anim;
    private Bullet bullet;
    private Unit unit;
    private GameObject health;
    private Text text;
    private GameMenu gamemenu;
    private AudioClip jumpsound;
    private AudioClip fireballsound;
    private AudioClip meowsound;
    private AudioSource Audios;

    public ActionState State
    {
        get { return (ActionState)Anim.GetInteger("State"); }
        set { Anim.SetInteger("State", (int) value); }
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
        unit = GetComponent<Unit>();
        gamemenu = FindObjectOfType<GameMenu>();
        Hp = PlayerPrefs.GetInt("Hp");
        health = GameObject.Find("Health");
        text = health.GetComponentInChildren<Text>();
        text.text = Hp.ToString();
        WalkCD = 0;
        jumpsound = Resources.Load<AudioClip>("Jump");
        fireballsound = Resources.Load<AudioClip>("Fireball");
        meowsound = Resources.Load<AudioClip>("Meow");
        Audios = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        if ((RB.velocity == Vector2.zero) || (State == ActionState.Cast)) State = ActionState.Idle;
        if (CoolDown > 0) CoolDown -= Time.deltaTime;
        if (WalkCD > 0) WalkCD -= Time.deltaTime;
    }

    void Update()
    {
        if (Input.GetButton("Horizontal") && !gamemenu.PAUSE) Walk();
        if (Input.GetButtonDown("Jump") && !gamemenu.PAUSE) Jump();
        if (Input.GetButtonDown("Fire1") && !gamemenu.PAUSE) Cast();
        if (Input.GetButtonDown("Cancel")) gamemenu.Pause();
    }

    void Walk()
    {
        if (WalkCD <= 0)
        {
            if ((RB.velocity == Vector2.zero) && (State != ActionState.Cast)) State = ActionState.Walk;
            Vector3 direction = transform.right * Input.GetAxis("Horizontal");
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, WalkSpeed * Time.deltaTime);
            Sprite.flipX = direction.x < 0.0F;
        }
    }

    void Jump()
    {
        if (RB.velocity == Vector2.zero)
        {
            if (State!=ActionState.Cast) State = ActionState.Jump;
            RB.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
            Audios.PlayOneShot(jumpsound, 0.5f);
        }
    }

    void Cast()
    {
        if (CoolDown <= 0)
        {
            State = ActionState.Cast;
            Vector3 position = transform.position;
            if (Sprite.flipX) position.x -= 0.5f;
                else position.x += 0.5f;
            Bullet NewBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
            NewBullet.Parent = gameObject;
            NewBullet.Direction = NewBullet.transform.right * (Sprite.flipX ? -1 : 1);
            NewBullet.Speed = 10;
            NewBullet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Fireball");
            CoolDown = 1;
            Audios.PlayOneShot(fireballsound, 0.5f);
        }
    }

    public override void ReceiveDamage()
    {
        WalkCD = 0.5f;
        if (unit.Directimpact==0)
        {
            RB.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
            RB.AddForce(Vector2.left * 3, ForceMode2D.Impulse);
        }
        else if(unit.Directimpact==1)
        {
            RB.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
            RB.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
        }
        Hp--;
        HpRefresh();
        if (Hp < 1)
        {
            SceneManager.LoadScene("GameOver");
        }else Audios.PlayOneShot(meowsound, 0.5f);
    }

    public void HpRefresh()
    {
       text.text = Hp.ToString();
    }

}

public enum ActionState
{
    Idle,
    Walk,
    Jump,
    Cast
}
