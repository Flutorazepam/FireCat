using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;

    private void Awake()
    {
        if (!target) target= FindObjectOfType<Character>().transform;
    }

    void FixedUpdate ()
    {
        Vector3 position = target.position; position.z = -10; position.y += 1;
        transform.position = Vector3.Lerp(transform.position, position, 3*Time.deltaTime);
	}
}
