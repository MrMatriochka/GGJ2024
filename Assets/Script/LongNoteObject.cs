using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteObject : MonoBehaviour
{
    bool canBePressed;
    public KeyCode keyToPress;

    enum States { Default, Pressed, Missed };
    States myState;
    void Start()
    {
        myState = States.Default;
    }
    private void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                if (Mathf.Abs(transform.position.y) > 0.25)
                {
                    GameManager.instance.NormalHit();
                }
                else if (Mathf.Abs(transform.position.y) > 0.05f)
                {
                    GameManager.instance.GoodHit();
                }
                else
                {
                    GameManager.instance.PerfectHit();
                }
                myState = States.Pressed;

                //gameObject.SetActive(false);
            }
        }

        if (Input.GetKey(keyToPress) && canBePressed)
        {
            GameManager.instance.score += GameManager.instance.scorePerTicLongNote;
        }

        if (Input.GetKeyUp(keyToPress) && myState == States.Pressed)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = true;
            collision.GetComponent<ButtonController>().canBePressed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator" && myState != States.Pressed)
        {
            collision.GetComponent<ButtonController>().canBePressed = false;
            if (myState != States.Pressed)
            {
                canBePressed = false;
                myState = States.Missed;
                GameManager.instance.NoteMissed();
            }
        }
    }
}
