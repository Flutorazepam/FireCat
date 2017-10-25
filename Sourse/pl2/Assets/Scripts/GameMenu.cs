using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private GameObject gamemenu;
    private bool pause;
    public bool PAUSE { get { return pause; } }

    private void Awake()
    {
        gamemenu = GameObject.Find("GameMenu");
        gamemenu.SetActive(false);
        pause = false;
    }

    public void Pause()
    {
        if(pause==false)
        {
            Time.timeScale = 0;
            pause = true;
            gamemenu.SetActive(true);
        }else
        {
            Time.timeScale = 1;
            pause = false;
            gamemenu.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
	
}
