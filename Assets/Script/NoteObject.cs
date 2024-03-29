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
                if (Mathf.Abs(transform.position.y-1.6f) > 0.5)
                {
                    GameManager.instance.NormalHit();
                }
                else if (Mathf.Abs(transform.position.y -1.6f) > 0.2)
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
        float tempo = GameManager.instance.beatScroller.beatTempo;
        while (t<1f)
        {
            t += Time.deltaTime * dissolveSpeed;
            transform.localPosition += new Vector3(0, tempo * Time.deltaTime, 0);
            GetComponent<Renderer>().material.SetFloat("_Dissolve", t);
            yield return null;
        }
        gameObject.SetActive(false);
        yield return null;
    }
}
