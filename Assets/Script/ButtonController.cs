using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    SpriteRenderer image;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress;
    void Start()
    {
        image = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            image.sprite = pressedImage;
        }
        if (Input.GetKeyUp(keyToPress))
        {
            image.sprite = defaultImage;
        }
    }
}
