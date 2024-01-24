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

    public int scorePerNote;
    public int scorePerGoodNote;
    public int scorePerPerfectNote;
    int score;
    public TMP_Text scoreText;

    [Header("multiplier")]
    int currentMultiplier;
    int multiplierTracker;
    public int[] multiplierThresholds;
    public TMP_Text multiplierText;
    public Slider multiplierSlider;
    void Start()
    {
        instance = this;
        currentMultiplier = 1;
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
    }

    public void NoteHit()
    {
        score += scorePerNote * currentMultiplier;
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
}
