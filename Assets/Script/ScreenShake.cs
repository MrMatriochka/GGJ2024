using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shake = 0;
    public float shakeAmount = 0.05f;
    public float decreaseFactor = 1;

    void Update()
    {
        if (shake > 0)
        {
            transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shake = 0.0f;
        }
    }
}
