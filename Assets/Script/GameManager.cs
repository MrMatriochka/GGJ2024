using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AudioSource music;
    public bool startPlaying;
    public BeatScroller beatScroller;
    public static GameManager instance;

    [Header("Score")]
    public int scorePerTicLongNote;
    public int scorePerNote;
    public int scorePerGoodNote;
    public int scorePerPerfectNote;
    [HideInInspector]public int score;
    public TMP_Text scoreText;

    [Header("Multiplier")]
    public int[] multiplierThresholds;
    public TMP_Text multiplierText;
    public Slider multiplierSlider;
    int currentMultiplier;
    int multiplierTracker;
    [HideInInspector] public int bonusMultiplier;

    [Header("Card")]
    public Card[] allCard;
    public GameObject cardPrefab;
    public int inventorySize;
    public GameObject[] inventorySlot;
    Card[] cardInventory;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentMultiplier = 1;
        bonusMultiplier = 1;
        cardInventory = new Card[inventorySize];
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                beatScroller.hasStarted = true;

                music.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetCard();
        }
    }

    void GetCard()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (cardInventory[i] == null)
            {
                cardInventory[i] = allCard[Random.Range(0, allCard.Length - 1)];
                UpdateInventorySlot(i);
                return;
            }
        }
    }
    void GetCard(Card card)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (cardInventory[i] == null)
            {
                cardInventory[i] = card;
                UpdateInventorySlot(i);
                return;
            }
        }
    }
    public void UseCard(int slotId)
    {
        if (cardInventory[slotId] != null)
        {
            //cardInventory[slotId].function;
            cardInventory[slotId] = null;
            UpdateInventorySlot(slotId);
        }
        
    }

    void UpdateInventorySlot(int i)
    {
            if (cardInventory[i] == null && inventorySlot[i].transform.childCount != 0)
            {
                Destroy(inventorySlot[i].transform.GetChild(0).gameObject);
            }
            else if (cardInventory[i] != null && inventorySlot[i].transform.childCount == 0)
            {
                GameObject card = Instantiate(cardPrefab, inventorySlot[i].transform);
                card.GetComponent<CardDisplay>().card = cardInventory[i];
                card.GetComponent<CardDisplay>().Display();
            }
            else if (cardInventory[i] != null && inventorySlot[i].transform.childCount != 0)
            {
                Destroy(inventorySlot[i].transform.GetChild(0).gameObject);
                GameObject card = Instantiate(cardPrefab, inventorySlot[i].transform);
                card.GetComponent<CardDisplay>().card = cardInventory[i];
                card.GetComponent<CardDisplay>().Display();
            }
    }
    public void NoteHit()
    {
        score += scorePerNote * currentMultiplier * bonusMultiplier;
        scoreText.text = "Score: " + score;
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;
            multiplierSlider.value = multiplierTracker;
            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
                multiplierText.text = "X " + currentMultiplier;
                multiplierSlider.maxValue = multiplierThresholds[currentMultiplier - 1];
            }
        }
    }

    public void NormalHit()
    {
        score += scorePerNote * currentMultiplier;
        NoteHit();
    }
    public void GoodHit()
    {
        score += scorePerGoodNote * currentMultiplier;
        NoteHit();
    }
    public void PerfectHit()
    {
        score += scorePerPerfectNote * currentMultiplier;
        NoteHit();
    }
    public void NoteMissed()
    {
        print("miss");

        multiplierTracker = 0;
        currentMultiplier = 1;
        multiplierText.text = "X " + currentMultiplier;
        multiplierSlider.value = multiplierTracker;
        multiplierSlider.maxValue = multiplierThresholds[currentMultiplier - 1];
    }
    public void NoteMissClick()
    {
        print("missClick");

        multiplierTracker = 0;
        currentMultiplier = 1;
        multiplierText.text = "X " + currentMultiplier;
        multiplierSlider.value = multiplierTracker;
        multiplierSlider.maxValue = multiplierThresholds[currentMultiplier - 1];
    }
}
