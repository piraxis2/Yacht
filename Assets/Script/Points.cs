using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Seteddice
{
    public int idx = -1;
    public int dicepoint = 0;
    public bool passible= false;
}


public class Points : MonoBehaviour
{

    static Points s_points;
    static public Points instance
    {

        get
        {
            if (s_points == null)
            {
                s_points = FindObjectOfType<Points>();
                s_points.Init();
            }
            return s_points;
        }
    }



    private List<Text>[] m_pointstext = new List<Text>[2];
    private int[,] m_points = new int[2, 13];
    private int[] m_temppoints = new int[12];
    private bool m_turn = false;
    private List<Seteddice>[] m_seteddices = new List<Seteddice>[2];
    private List<Button>[] m_buttons = new List<Button>[2];

    private Transform[] m_backimage = new Transform[2];

    private Vector3 m_oripos;
    private Vector3 m_orisize;
    private int m_currturncount = 0;
    private Button m_restartbutton;

    public List<Button>[] GButtons
    {
        get { return m_buttons; }
    }

    private void Init()
    {
        m_oripos = transform.localPosition;
        m_orisize = transform.localScale;
        for(int i = 0; i<2;i++)
        {
            m_pointstext[i] = new List<Text>();
            m_buttons[i] = new List<Button>();
            m_seteddices[i] = new List<Seteddice>();
        }
        m_pointstext[0].AddRange(transform.Find("Points/1pPoints").GetComponentsInChildren<Text>());
        m_pointstext[1].AddRange(transform.Find("Points/2pPoints").GetComponentsInChildren<Text>());
        m_buttons[0].AddRange(transform.Find("Points/1pPoints").GetComponentsInChildren<Button>(true));
        m_buttons[1].AddRange(transform.Find("Points/2pPoints").GetComponentsInChildren<Button>(true));

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                m_seteddices[i].Add(new Seteddice());
                m_seteddices[i][j].idx = j;
                int idx = j;
                m_buttons[i][j].onClick.AddListener(() => { Buttons(idx); });
                m_buttons[i][j].interactable = (i == 0);
            }
            m_seteddices[i].Add(new Seteddice());
            m_seteddices[i][12].idx = 12;

        }

        for (int i = 0; i < 12; i++)
        {
            m_temppoints[i] = 0;
        }

        m_restartbutton = transform.Find("Restart").GetComponent<Button>();
        m_restartbutton.onClick.AddListener(() => Restart());

        m_backimage[0] = transform.Find("1P");
        m_backimage[1] = transform.Find("2P");
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
            case "Choice": return m_pointstext[p][6];
            case "4ofaKind": return m_pointstext[p][7];
            case "FullHouse": return m_pointstext[p][8];
            case "S.Straight": return m_pointstext[p][9];
            case "L.Straight": return m_pointstext[p][10];
            case "Yacht": return m_pointstext[p][11];
            case "SubTotal": return m_pointstext[p][12];
            case "Bonus": return m_pointstext[p][13];
            case "Total": return m_pointstext[p][14];
        }
        return null;

    }



    public void Check()
    {

        for (int i = 0; i < m_temppoints.Length; i++)
        {
            m_temppoints[i] = 0;
        }
        int[] dices = new int[5];

        for (int i = 0; i < 5; i++)
        {
            dices[i] = Dice.instance.dice(i);
        }


        int[] fullhouscheck = new int[6];

        for (int i = 0; i < 6; i++)
        {
            int count = 0;
            int fourofkind = 0;
            for (int j = 0; j < 5; j++)
            {

                if (dices[j] == i + 1)
                    count++;
                else
                    fourofkind = dices[j];
            }
            fullhouscheck[i] = count;

            m_temppoints[i] = count * (i + 1);

            if (count >= 4)
            {
                m_temppoints[7] = (4 * (i + 1)) + fourofkind;
            }

            if (count >= 5)
            {
                m_temppoints[11] = 50;

                m_temppoints[8] = 5 * (i + 1);
            }

        }

        bool two = false;
        bool three = false;

        for (int i = 0; i < 6; i++)
        {
            if (fullhouscheck[i] == 3)
                three = true;

            if (fullhouscheck[i] == 2)
                two = true;
        }


        int choice = 0;

        for (int i = 0; i < 5; i++)
        {
            choice += dices[i];
        }

        m_temppoints[6] = choice;

        if (two && three)
        {
            m_temppoints[8] = choice;
        }


        List<int> sortlist = new List<int>();

        sortlist.AddRange(dices);
        sortlist.Sort();

        int temp = sortlist[0];
        int tempcheck = 0;

        for (int i = 0; i < 4; i++)
        {
            if (sortlist[i] + 1 != sortlist[i + 1])
            {
                tempcheck++;
            }

        }

        if (tempcheck == 0)
            m_temppoints[10] = 30;



        for (int i = 0; i < 3; i++)
        {
            int count = 0;
            for (int j = 0; j < 4; j++)
            {
                if (fullhouscheck[j + i] > 0)
                    count++;
            }

            if (count >= 4)
                m_temppoints[9] = 15;

        }


        VisualrizePoint();

    }




    public void SumPoints()
    {

        int topsum = 0;
        for (int i = 0; i < 6; i++)
        {
            topsum += m_seteddices[TurnMng.instance.intturn][i].dicepoint;
        }
        m_pointstext[TurnMng.instance.intturn][12].text = topsum.ToString() + " / " + 63.ToString();

        if (topsum >= 63)
        {
            m_seteddices[TurnMng.instance.intturn][12].dicepoint = 35;
            m_pointstext[TurnMng.instance.intturn][13].text = 35.ToString();
        }
       
        int totalsum = 0;
        for (int i = 0; i < 13; i++)
        {
            totalsum += m_seteddices[TurnMng.instance.intturn][i].dicepoint;
        }
        m_pointstext[TurnMng.instance.intturn][14].text = totalsum.ToString();

    }



    public void GameEnd()
    {
        StartCoroutine(IEendPase());
    }


    public void Restart()
    {
        transform.localPosition = m_oripos;
        transform.localScale = m_orisize;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                m_seteddices[i][j].dicepoint = 0;
                m_seteddices[i][j].passible = false;
            }
            for (int j = 0; j < 15; j++)
            {
                m_pointstext[i][j].text = "";
            }
            m_pointstext[i][12].text = "0 / 63";
        }

        TurnMng.instance.ReStart();
        DiceTray.instance.EmptyTray();
        m_restartbutton.gameObject.SetActive(false);

        for (int j = 0; j < 12; j++)
        {
            m_buttons[0][j].interactable = true;
            m_buttons[1][j].interactable = false;
            m_pointstext[0][j].color = new Color(m_pointstext[0][j].color.r, m_pointstext[0][j].color.g, m_pointstext[0][j].color.b, 0.4f);
            m_pointstext[1][j].color = new Color(m_pointstext[1][j].color.r, m_pointstext[1][j].color.g, m_pointstext[1][j].color.b, 0.4f);
        }
        Dice.instance.ResetRollcout();

    }

    public void VisualrizePoint()
    {
        int turn = TurnMng.instance.intturn;

        for (int i = 0; i < m_temppoints.Length; i++)
        {
            if (m_seteddices[turn][i].passible)
                continue;

            m_pointstext[turn][i].text = string.Empty;
        }

        for (int i = 0; i < m_temppoints.Length; i++)
        {

            if (m_seteddices[turn][i].passible)
                continue;

            if (m_temppoints[i] >= 0)
            {
                m_pointstext[turn][i].text = m_temppoints[i].ToString();
            }
                
        }
    }



    public void Buttons(int idx)
    {

        if (TurnMng.instance.Gameend)
            return;

        if (Dice.instance.Rollcount >= 3)
            return;

        if (Dice.instance.DiceRolling)
            return;

        int turn = TurnMng.instance.boolturn ? 1 : 0;
        if (m_seteddices[turn][idx].passible)
            return;

        m_seteddices[turn][idx].passible = true;
        m_seteddices[turn][idx].dicepoint = m_temppoints[idx];

        m_pointstext[turn][idx].color = new Color(m_pointstext[turn][idx].color.r, m_pointstext[turn][idx].color.g, m_pointstext[turn][idx].color.b, 1f);

        for (int i = 0; i < m_temppoints.Length; i++)
        {
            if (m_seteddices[turn][i].passible)
                continue;

            m_pointstext[turn][i].text = string.Empty;
        }
        DiceTray.instance.EmptyTray();
        for (int i = 0; i < 5; i++)
        {
            Dice.instance.Ani(i).SetInteger("Diceroll", 7);
            Dice.instance.dicezero();
        }
        Dice.instance.ResetRollcout();
      
        m_currturncount++;

        SumPoints();

        TurnMng.instance.ChangeTurn();
        m_backimage[0].gameObject.SetActive(!TurnMng.instance.boolturn);
        m_backimage[1].gameObject.SetActive(TurnMng.instance.boolturn);




        for (int j = 0; j < 12; j++)
        {
            m_buttons[0][j].interactable = !TurnMng.instance.boolturn;
            m_buttons[1][j].interactable = TurnMng.instance.boolturn;
        }



        if (m_currturncount>=2)
        {
            m_currturncount = 0;
            TurnMng.instance.NextTurn();
        }

    }





    private IEnumerator IEendPase()
    {

        float elapsedtime = 0;
        bool stop = false;
        
        while(!stop)
        {
            elapsedtime += Time.deltaTime * 2;

            transform.localPosition = Vector3.Lerp(m_oripos, new Vector3(0, 0, 0), elapsedtime);
            transform.localScale = Vector3.Lerp(m_orisize, new Vector3(3, 3, 1), elapsedtime);

            if(elapsedtime>=1)
            {
                stop = true;
                m_restartbutton.gameObject.SetActive(true);
            }

            yield return null;
        }




        yield return null;
    }

}
