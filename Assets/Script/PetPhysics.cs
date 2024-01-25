using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPhysics : MonoBehaviour
{
    public float power;
    public float upwardForce;
    public float radius;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Prout();
        }
    }

    void Prout()
    {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null && rb.tag == "Decor")
                    rb.AddExplosionForce(power, explosionPos, radius, upwardForce);
            }
    }
}
