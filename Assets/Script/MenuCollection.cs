using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCollection : MonoBehaviour
{
    public int row;
    public int collum;
    public GameObject prefab;
    public float spacingX;
    public float spacingY;


    public Card[] allCard;
    void Start()
    {
        GenerateCollection();
    }

    void GenerateCollection()
    {
        float posX = 0;
        float posY = 0;
        Vector3 spawnPos;
        int objSpawned = 0;
        for (int e = 0; e < row; e++)
        {
            for (int i = 0; i < collum; i++)
            {
                spawnPos = new Vector3(posX, posY, 0);
                GameObject obj = Instantiate(prefab, transform);
                obj.transform.localPosition = spawnPos;

                obj.GetComponent<CardDisplay>().card = allCard[objSpawned];
                obj.GetComponent<CardDisplay>().Display();

                if (!PlayerPrefs.HasKey(allCard[objSpawned].name))
                {
                    obj.GetComponent<CardDisplay>().lockedDecal.enabled = true;
                }

                posX += spacingX;
                objSpawned++;
                if (objSpawned >= allCard.Length)
                {
                    return;
                }
            }
            posX = 0;
            posY -= spacingY;
        }
    }
}
