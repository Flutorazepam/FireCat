using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Character character;
    private AudioSource audios;

    private void Awake()
    {
        character = FindObjectOfType<Character>();
        audios = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            character.GetComponent<Rigidbody2D>().AddForce(Vector2.up*10, ForceMode2D.Impulse);
            audios.Play();
        }
    }

}
