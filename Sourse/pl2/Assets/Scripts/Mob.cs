using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Unit
{
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();
            if (unit && unit is Character)
            {
                if (gameObject.transform.position.x - unit.transform.position.x > 0) unit.Directimpact = 0;
                else unit.Directimpact = 1;
                unit.ReceiveDamage();
                
            }
        }
    }
}
