using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap1 : MonoBehaviour
{
    private bool TrapOn;
    private Vector3 position;
    private float TrapTime;

    private void Awake()
    {
        TrapOn = false;
        TrapTime = 0;
        position = transform.position; position.y = transform.position.y - 2;
    }

    private void FixedUpdate()
    {
        if (!TrapOn && TrapTime > 0) TrapTime -= Time.deltaTime;
        if (TrapTime < 0) TrapOn = true;
        if (TrapOn) Move();
        if (transform.position == position) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TrapTime = 1;  
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime);
    }

}
