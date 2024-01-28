using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
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
                if(Mathf.Abs(transform.localPosition.y) > 0.25)
                {
                    GameManager.instance.NormalHit();
                }
                else if (Mathf.Abs(transform.localPosition.y) > 0.05f)
                {
                    GameManager.instance.GoodHit();
                }
                else
                {
                    GameManager.instance.PerfectHit();
                }
                myState = States.Pressed;

                StartCoroutine(Dissolve());
                
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Activator")
        {
            canBePressed = true;
            collision.GetComponent<ButtonController>().canBePressed = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Activator" && myState != States.Pressed)
        {
            collision.GetComponent<ButtonController>().canBePressed = false;
            if(myState != States.Pressed)
            {
                canBePressed = false;
                myState = States.Missed;
                GameManager.instance.NoteMissed();
            }
        }
    }

    public float dissolveSpeed;
    IEnumerator Dissolve()
    {
        float t = 0;
        while (t<1f)
        {
            t += Time.deltaTime * dissolveSpeed;
            GetComponent<Renderer>().material.SetFloat("_Dissolve", t);
            yield return null;
        }
        gameObject.SetActive(false);
        yield return null;
    }
}
