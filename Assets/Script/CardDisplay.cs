using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.SceneManagement;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public DecalProjector nameDecal;
    public DecalProjector iconDecal;
    public DecalProjector lockedDecal;
    public TMP_Text description;

    public void Display()
    {
        nameDecal.material = card.nameDecalMat;
        iconDecal.material = card.iconDecalMat;
        description.text = card.description;
    }
    private void OnMouseEnter()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            description.transform.parent.gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            description.transform.parent.gameObject.SetActive(false);
        }
    }
}
