using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class ButtonController : MonoBehaviour
{
    public SpriteRenderer image;

    public KeyCode keyToPress;

    public GameObject spotLight;

    [HideInInspector]public bool canBePressed;
    public VisualEffect VFX;
    AudioSource SFX;

    private void Start()
    {
        SFX = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress) || Input.GetKeyDown(KeyCode.Space))
        {
            image.enabled = true;
            VFX.Play();
            SFX.Play();
            SFX.pitch = Random.Range(.9f, 1.1f);
            spotLight.SetActive(true);
            if (!canBePressed)
            {
                GameManager.instance.NoteMissClick();
            }
        }
        if (Input.GetKeyUp(keyToPress) || Input.GetKeyUp(KeyCode.Space))
        {
            image.enabled = false;
            spotLight.SetActive(false);
        }
    }
}
