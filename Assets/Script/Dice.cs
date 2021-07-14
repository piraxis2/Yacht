using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{




    private int[] m_dice = new int[5];



    public int dice(int idx)
    {
        if (idx < 0 || idx > 4)
        {
            return -1;
        }

        return m_dice[idx];
    }

    public void DiceRoll()
    {

        for (int i = 0; i < 5; i++)
        {
            m_dice[i] = Random.Range(1, 6);
        }

    }

}
