using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public DecalProjector nameDecal;
    public DecalProjector iconDecal;
    public DecalProjector lockedDecal;

    public void Display()
    {
        nameDecal.material = card.nameDecalMat;
        iconDecal.material = card.iconDecalMat;
    }
}
