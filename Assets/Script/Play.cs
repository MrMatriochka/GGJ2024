using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{

    public GameManager manager;

    public AudioSource audioSource;
    
    // Start is called before the first frame update
    public void Begin()
    {
        manager.Begin();
    }

    public void Footstep()
    {
        audioSource.Play();
        audioSource.pitch = Random.Range(0.9f, 1.1f);
    }
}
