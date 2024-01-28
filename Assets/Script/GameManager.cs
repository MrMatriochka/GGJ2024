using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.Rendering.Universal;

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
    List<Card> unlockedCard = new List<Card>();
    public GameObject cardPrefab;
    public int inventorySize;
    public GameObject[] inventorySlot;
    [HideInInspector] public Card[] cardInventory;
    public float timeBetweenCard;
    float timeToNextCard;
    public float cdReducePerPefectHit;

    [Header("Toxicity")]
    public Slider toxicitySlider;
    public Volume toxicityVignette;
    public Image toxicityMist;
    Vignette vg;
    [HideInInspector] public float toxicity;
    public float toxicityGainPerMiss;
    public float toxicityGainPerMissClick;
    [HideInInspector] public bool shield;
    [HideInInspector] public int missShield;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("FirstLaunch"))
        {
            foreach (Card card in allCard)
            {
                if (PlayerPrefs.HasKey(card.name) && !card.unlocked)
                {
                    card.unlocked = true;
                }
            }
        }
        else
        {
            PlayerPrefs.SetInt("FirstLaunch", 1);
            foreach (Card card in allCard)
            {
                if (card.unlocked)
                {
                    PlayerPrefs.SetInt(card.name, 1);
                }
            }
        }

        if(!music.isPlaying && startPlaying && !gameOver)
        {
            EndStats();
        }
    }
    void Start()
    {

        allCard[16].dropRate = 0;
        currentMultiplier = 1;
        bonusMultiplier = 1;
        cardInventory = new Card[inventorySize];
        timeToNextCard = timeBetweenCard;
        toxicityVignette.sharedProfile.TryGet(out vg);
        UpdateUnlockedCard();

        vg.intensity.value = toxicity / 100;
        toxicityMist.material.SetFloat("_Opacity", vg.intensity.value);
    }

    // Update is called once per frame
    void Update()
    {

        if (timeToNextCard <= 0)
        {
            GetCard();
            timeToNextCard = timeBetweenCard;
        }
        else
        {
            timeToNextCard -= Time.deltaTime;
        }

        if (toxicity>=100)
        {
            GameOver();
        }

        //tomate dropRate
        if (toxicity >= 90)
        {
            allCard[16].dropRate = 0.5f;
        }
        else if(toxicity >= 75)
        {
            allCard[16].dropRate = 0.35f;
        }
        else if (toxicity >= 50)
        {
            allCard[16].dropRate = 0.2f;
        }
        else if (toxicity >= 25)
        {
            allCard[16].dropRate = 0.1f;
        }
        else { allCard[16].dropRate = 0; }
    }

    public GameObject gameOverMenu;
    bool gameOver;
    void GameOver()
    {
        gameOverMenu.SetActive(true);
        beatScroller.hasStarted = false;
        music.Stop();
        gameOver = true;
    }

    public void Begin()
    {
        startPlaying = true;
        beatScroller.hasStarted = true;

        music.Play();
    }
    void UpdateUnlockedCard()
    {
        unlockedCard.Clear();
        foreach (Card card in allCard)
        {
            if (card.unlocked)
            {
                unlockedCard.Add(card);
            }
        }
    }
    void UnlockCard(Card card)
    {
        card.unlocked = true;
        PlayerPrefs.SetInt(card.name,1);
        PlayerPrefs.Save();
        UpdateUnlockedCard();
    }
    public void GetCard()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (cardInventory[i] == null)
            {
                cardInventory[i] = GetRandomCard();
                UpdateInventorySlot(i);
                return;
            }
        }
    }
    Card GetRandomCard()
    {
        float totalWeight = 0;
        foreach (Card card in allCard)
        {
            totalWeight += card.dropRate;
        }
        
        float diceRoll = Random.Range(0f, totalWeight);

        foreach (Card card in allCard)
        {
            if (card.dropRate >= diceRoll)
            {
                return card;
            }

            diceRoll -= card.dropRate;
        }

        // As long as everything works we'll never reach this point, but better be notified if this happens!
        throw new System.Exception("Reward generation failed!");
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
            Card card = cardInventory[slotId];
            CardFunctions.instance.CallFunction(card.function, card.timer,card.integer);
            cardInventory[slotId] = null;
            UpdateInventorySlot(slotId);
        }
        
    }

    public void UpdateInventorySlot(int i)
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
                card.transform.localPosition = Vector3.zero;
            }
            else if (cardInventory[i] != null && inventorySlot[i].transform.childCount != 0)
            {
                Destroy(inventorySlot[i].transform.GetChild(0).gameObject);
                GameObject card = Instantiate(cardPrefab, inventorySlot[i].transform);
                card.GetComponent<CardDisplay>().card = cardInventory[i];
                card.GetComponent<CardDisplay>().Display();
                card.transform.localPosition = Vector3.zero;
            }
    }
    public void NoteHit()
    {
        score += scorePerNote * currentMultiplier * bonusMultiplier;
        scoreText.text = "Score: " + score;
        hitNoteNb++;
        totalNoteNb++;
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
        timeToNextCard -= cdReducePerPefectHit;
        NoteHit();
    }
    public void NoteMissed()
    {
        totalNoteNb++;
        if (missShield > 0)
        {
            missShield--;
        }
        if (!shield && missShield <=0)
        {
            toxicity += toxicityGainPerMiss;
            toxicitySlider.value = toxicity;
            vg.intensity.value = toxicity / 100;
            toxicityMist.material.SetFloat("_Opacity", vg.intensity.value);
            multiplierTracker = 0;
            currentMultiplier = 1;
            multiplierText.text = "X " + currentMultiplier;
            multiplierSlider.value = multiplierTracker;
            multiplierSlider.maxValue = multiplierThresholds[currentMultiplier - 1];
        } 
    }
    public void NoteMissClick()
    {
        if (missShield > 0)
        {
            missShield--;
        }
        if (!shield && missShield <= 0)
        {
            toxicity += toxicityGainPerMissClick;
            toxicitySlider.value = toxicity;
            vg.intensity.value = toxicity / 100;
            toxicityMist.material.SetFloat("_Opacity", vg.intensity.value);
            multiplierTracker = 0;
            currentMultiplier = 1;
            multiplierText.text = "X " + currentMultiplier;
            multiplierSlider.value = multiplierTracker;
            multiplierSlider.maxValue = multiplierThresholds[currentMultiplier - 1];
        }
    }

    [Header("Stats")]
    public TMP_Text endScoreText;
    public TMP_Text precisionText;
    public GameObject newHighScore;
    int totalNoteNb;
    int hitNoteNb;
    public GameObject endMenu;
    void EndStats()
    {
        endMenu.SetActive(true);
        if (PlayerPrefs.HasKey(music.clip.name))
        {
            if (PlayerPrefs.GetInt(music.clip.name) < score)
            {
                PlayerPrefs.SetInt(music.clip.name, score);
                PlayerPrefs.Save();
                newHighScore.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt(music.clip.name, score);
            PlayerPrefs.Save();
            newHighScore.SetActive(true);
        }
        endScoreText.text = "Score: " + score.ToString();
        int precision = Mathf.RoundToInt((hitNoteNb / totalNoteNb) * 100);
        precisionText.text = "Accuracy: " + precision.ToString();
    }
}
