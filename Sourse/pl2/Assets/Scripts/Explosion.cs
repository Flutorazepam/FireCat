using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float cd;

	void Start ()
    {
        cd = 0.5f;	
	}
    private void Update()
    {
        cd-=Time.deltaTime;
        if (cd < 0) Destroy(gameObject);
    }

}
