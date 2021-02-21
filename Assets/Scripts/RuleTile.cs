using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleTile : MonoBehaviour
{
    int value;
    public Color satisfied;

    public void setSatisfied(bool x)
    {
        if (x)
        {
            GetComponent<SpriteRenderer>().color = satisfied;
            
        } else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void setValue(int x)
    {
        value = x;
    }

    public int getValue()
    {
        return value;
    }
}
