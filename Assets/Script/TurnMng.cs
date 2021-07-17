using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnMng : MonoBehaviour
{

    private static TurnMng s_turnmng;
    public static TurnMng instance
    {
        get
        {
            if(s_turnmng==null)
            {
                s_turnmng = FindObjectOfType<TurnMng>();
                s_turnmng.init();
            }

            return s_turnmng;
        }
    }


    private int m_turn = 1;
    private bool m_boolturn = false;

    private bool m_gameend = false;
    private Text m_turntext;
    


    private void init()
    {
        m_turntext = GetComponent<Text>();
    }


    public int intturn
    {
        get { return m_boolturn ? 1 : 0; }
    }

    public bool boolturn
    {
        get { return m_boolturn; }
    }

    public bool Gameend
    {
        get { return m_gameend; }
    }

    public void ChangeTurn()
    {
        m_boolturn = !m_boolturn;
    }


    public void ReStart()
    {
        m_turn = 1;
        m_boolturn = false;
        m_gameend = false;
        m_turntext.text = "Turn\n" + "1 / 12";
    }

    public void NextTurn()
    {
     
        m_turn++;
        if (m_turn > 12)
        {
            m_gameend = true;
            Points.instance.GameEnd();

        }
        else
            m_turntext.text = "Turn\n" + m_turn.ToString() + " / 12";

    }


}
