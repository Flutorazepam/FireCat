using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpBar : MonoBehaviour
{
    private Transform[] HP = new Transform[10];
    private Boss boss;

    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
        for(int i=0; i< HP.Length;i++)
        {
            HP[i] = transform.GetChild(i);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < HP.Length; i++)
        {
            if (i < boss.Hp) HP[i].gameObject.SetActive(true);
            else HP[i].gameObject.SetActive(false);
        }
    }
}
