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
    private void Awake()
    {
        instance = this;
    }
    public void CallFunction(Function function,float timer,int integer)
    {
        switch (function)
        {
            case Function.None:
                break;
            case Function.TimerShield:
                break;
            case Function.MissShield: 
                break;
            case Function.Reroll:
                break; 
            case Function.ReduceToxicity:
                break;
            case Function.SlowMo:
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
        GameManager.instance.bonusMultiplier *= multiplier;
        yield return new WaitForSeconds(timer);
        GameManager.instance.bonusMultiplier /= multiplier;
        yield return null;
    }
}
