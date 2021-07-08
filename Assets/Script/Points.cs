using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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



    private List<Text>[] m_points = new List<Text>[2];

    private void Init()
    {
        for(int i = 0; i<2;i++)
        {
            m_points[i] = new List<Text>();
        }


        m_points[0].AddRange(transform.Find("1pPoints").GetComponentsInChildren<Text>());
        m_points[1].AddRange(transform.Find("2pPoints").GetComponentsInChildren<Text>());



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
            case "Aces": return m_points[p][0]; 
            case "Deuces": return m_points[p][1];
            case "Threes": return m_points[p][2];
            case "Fours": return m_points[p][3];
            case "Fives": return m_points[p][4];
            case "Sixes": return m_points[p][5];
            case "SubTotal": return m_points[p][6];
            case "Bonus": return m_points[p][7];
            case "Choice": return m_points[p][8];
            case "4ofaKind": return m_points[p][9];
            case "FullHouse": return m_points[p][10];
            case "S.Straight": return m_points[p][11];
            case "L.Straight": return m_points[p][12];
            case "Yacht": return m_points[p][13];
            case "Total": return m_points[p][14];
        }
        return null;


    }

    public void debug()
    {
       Points s =  Points.instance;
        AccessPoint("Aces", 1).text = "yes";
        AccessPoint("Yacht", 2).text = "ni";


    }


}
