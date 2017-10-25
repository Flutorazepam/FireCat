using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float speed = 10;
    public float Speed { set { speed = value; } }

    private Vector3 direction;
    public Vector3 Direction { set { direction = value; } get { return direction; } }

    private GameObject parent;
    public GameObject Parent { set { parent = value; } get { return parent; } }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag!=parent.tag)
        {
            Destroy(gameObject);
        }
        Unit unit = collider.GetComponent<Unit>();
        if (unit && unit.tag != parent.tag)
        {
            if (parent.transform.position.x - unit.transform.position.x > 0) unit.Directimpact = 0;
            else unit.Directimpact = 1;
            unit.ReceiveDamage();
        }
        else if (collider.gameObject.tag == "DestroyObject") Destroy(collider.gameObject);
        
    }


    void Start ()
    {
        Destroy(gameObject, 2);
	}
	

	void Update ()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        if (!parent) Destroy(gameObject);
	}
}
