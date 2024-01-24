using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public TMP_Text nameText;
    public Image icon;
    public void Display()
    {
        nameText.text = card.name;
        icon.sprite = card.icon;
    }
}
