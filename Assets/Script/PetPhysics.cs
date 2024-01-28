using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPhysics : MonoBehaviour
{
    public float power;
    public GameObject[] decor;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Prout();
        }
    }

    void Prout()
    {
        foreach (GameObject obj in decor)
        {
            Vector3 direction = obj.transform.position-transform.position;
            direction = direction.normalized;
            obj.GetComponent<Rigidbody>().AddForce(direction*power, ForceMode.Impulse);
        }
    }
}
