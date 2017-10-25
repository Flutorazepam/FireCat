using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private Character character;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            if (SceneManager.GetActiveScene().buildIndex != 3)
            {
                PlayerPrefs.SetInt("Hp",character.Hp);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                
            }
            else
            {
                SceneManager.LoadScene("Victory");
            }        
        }
    }

    private void Awake()
    {
        character = FindObjectOfType<Character>();
    }


}
