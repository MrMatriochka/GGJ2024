using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    public KeyCode keyToPress;
    public int id;
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            GameManager.instance.UseCard(id);
        }
    }
}
