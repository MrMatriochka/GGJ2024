using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CardFunctions : MonoBehaviour
{
    public enum Function
    {
        None,
        TimerShield,
        MissShield,
        Reroll,
        ReduceToxicity,
        SlowMo,
        Multiplier
    }
    public static CardFunctions instance;
    GameManager gameManager;
    private void Awake()
    {
        //if (instance == null)
        //{
            //DontDestroyOnLoad(gameObject);
            instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
    private void Start()
    {
        gameManager = GameManager.instance;
    }
    public void CallFunction(Function function,float timer,int integer)
    {
        switch (function)
        {
            case Function.None:
                break;
            case Function.TimerShield:
                StartCoroutine(TimerShield(timer));
                break;
            case Function.MissShield:
                MissShield(integer);
                break;
            case Function.Reroll:
                Reroll();
                break; 
            case Function.ReduceToxicity:
                ReduceToxicity(integer);
                break;
            case Function.SlowMo:
                StartCoroutine(SlowMo(timer));
                break;
            case Function.Multiplier:
                StartCoroutine(Multiply(timer, integer));
                break;
            default:
                break;
        }
    }

    IEnumerator Multiply(float timer, int multiplier)
    {
        gameManager.bonusMultiplier *= multiplier;
        yield return new WaitForSeconds(timer);
        gameManager.bonusMultiplier /= multiplier;
        yield return null;
    }
    IEnumerator TimerShield(float timer)
    {
        gameManager.shield = true;
        yield return new WaitForSeconds(timer);
        gameManager.shield = false;
        yield return null;
    }
    IEnumerator SlowMo(float timer)
    {
        Time.timeScale = 0.75f;
        yield return new WaitForSeconds(timer);
        Time.timeScale = 1;
        yield return null;
    }
    void ReduceToxicity(int percent) 
    {
        gameManager.toxicity -= percent;
        if(gameManager.toxicity<0)
        {
            gameManager.toxicity = 0;
        }

        gameManager.toxicitySlider.value = gameManager.toxicity;
        gameManager.vg.intensity.value = gameManager.toxicity / 100;
        gameManager.toxicityMist.material.SetFloat("_Opacity", gameManager.vg.intensity.value);
    }
    void MissShield(int miss)
    {
        gameManager.missShield += miss;
    }
    void Reroll()
    {
        for (int i = 0; i < gameManager.inventorySize; i++)
        {
            if (gameManager.cardInventory[i] != null)
            {
                gameManager.cardInventory[i] = null;
                Destroy(gameManager.inventorySlot[i].transform.GetChild(0).gameObject);
                gameManager.GetCard();
            }
        }
    }
}
