using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public SpriteRenderer image;

    public KeyCode keyToPress;

    [HideInInspector]public bool canBePressed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            image.enabled = true;

            if (!canBePressed)
            {
                GameManager.instance.NoteMissClick();
            }
        }
        if (Input.GetKeyUp(keyToPress))
        {
            image.enabled = false;
        }
    }
}
