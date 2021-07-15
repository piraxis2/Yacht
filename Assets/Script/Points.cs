using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dicelol
{
    public int dicepoint = 0;
    public bool passiple = false;
}


public class Points : MonoBehaviour
{

    static Points s_points;
    static public Points instance
    {

        get {
            if (s_points == null)
            {
                s_points = FindObjectOfType<Points>();
                s_points.Init();
            }
            return s_points; }
    }



    private List<Text>[] m_pointstext = new List<Text>[2];
    private int[,] m_points = new int[2, 12];
    private int[] m_temppoints = new int[12];
    private bool m_turn = false;

    private void Init()
    {
        for(int i = 0; i<2;i++)
        {
            m_pointstext[i] = new List<Text>();
        }


        m_pointstext[0].AddRange(transform.Find("1pPoints").GetComponentsInChildren<Text>());
        m_pointstext[1].AddRange(transform.Find("2pPoints").GetComponentsInChildren<Text>());

        for (int i = 0; i < 12; i++)
        {
            m_temppoints[i] = -1;
        }

    }


    public Text AccessPoint(string name, int p)
    {

        if (p > 2)
            return null;

        if (p < 1)
            return null;

        p--;

        switch (name)
        {
            case "Aces": return m_pointstext[p][0]; 
            case "Deuces": return m_pointstext[p][1];
            case "Threes": return m_pointstext[p][2];
            case "Fours": return m_pointstext[p][3];
            case "Fives": return m_pointstext[p][4];
            case "Sixes": return m_pointstext[p][5];
            case "SubTotal": return m_pointstext[p][6];
            case "Bonus": return m_pointstext[p][7];
            case "Choice": return m_pointstext[p][8];
            case "4ofaKind": return m_pointstext[p][9];
            case "FullHouse": return m_pointstext[p][10];
            case "S.Straight": return m_pointstext[p][11];
            case "L.Straight": return m_pointstext[p][12];
            case "Yacht": return m_pointstext[p][13];
            case "Total": return m_pointstext[p][14];
        }
        return null;

    }



    public void Check()
    {

        int[] dices = new int[5];
        for (int i = 0; i < 5; i++)
        {
            dices[i] = Dice.instance.dice(i);
        }

        int choice = 0;

        for (int i = 0; i < 6; i++)
        {
            int count = 0; 
            for (int j = 0; j < 5; j++)
            {
                if (dices[j] == i + 1)
                    count++;
            }
            m_temppoints[i] = count * (i + 1);

            if(count>=4)
            {
                m_temppoints[7] = count * (i + 1);
            }

            if (count >= 5)
            {
                m_temppoints[11] = 50;
            }

            choice += dices[i];
        }

        m_temppoints[6] = choice;

        int count2 = 0;
        for (int i = 0; i < 5; i++)
        {


        }






    }

  







    public void debug()
    {
       Points s =  Points.instance;
        AccessPoint("Aces", 1).text = "yes";
        AccessPoint("Yacht", 2).text = "ni";


    }


}
