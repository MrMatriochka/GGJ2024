using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPhysics : MonoBehaviour
{
    public float power;
    public GameObject[] decor;
    public ScreenShake shake;
    public float shakeTime;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R) )
        {
            Prout();
            shake.shake = shakeTime;
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
