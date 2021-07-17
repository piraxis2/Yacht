using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{


    private static Dice s_dice;
    public static Dice instance
    {
        get
        {
            if(s_dice==null)
            {
                s_dice = FindObjectOfType<Dice>();
                s_dice.init();
            }

            return s_dice;
        }
    }


    public void Awake()
    {
        s_dice = FindObjectOfType<Dice>();
        s_dice.init();
    }

    private int[] m_dice = new int[5];

    private List<Animator> m_diceanis = new List<Animator>();
    private List<Button> m_buttons = new List<Button>();
    private Text[] m_cuptext = new Text[2];
    private bool m_dicelock = false;
    private bool m_dicerolling = false;
    private AudioSource m_sound;
    private int m_rollcount = 3;

    public int Rollcount
    {
        get { return m_rollcount; }
    }

    public bool DiceRolling
    {
        get { return m_dicerolling; }
    }


    public void ResetRollcout()
    {
        m_rollcount = 3;
        m_dicelock = false;
        m_dicerolling = false;
        LeftTexting(m_rollcount.ToString() + " Left");
    }

    public Animator Ani(int idx)
    {
        if (idx < m_diceanis.Count)
            return m_diceanis[idx];

        return null;
    }

    private void LeftTexting(string val)
    {
        for (int i = 0; i < 2; i++)
        {
            m_cuptext[i].text = val;
        }
    }

    private void init()
    {
        m_diceanis.AddRange(GetComponentsInChildren<Animator>(true));
        m_buttons.AddRange(GetComponentsInChildren<Button>(true));
        
        m_cuptext = transform.Find("Left").GetComponentsInChildren<Text>(true);
        m_buttons[5].onClick.AddListener(DiceRoll);
        m_sound = m_buttons[5].GetComponentInChildren<AudioSource>(true);
        LeftTexting("3 Left");

        for (int i = 0; i < 5; i++)
        {
            m_dice[i] = -1;
            int idx = i;
            m_buttons[i].onClick.AddListener(() => DiceButtonfunc(idx));
        }
    }


    public int dice(int idx)
    {
        if (idx < 0 || idx > 4)
        {
            return -1;
        }

        return m_dice[idx];
    }

    public void dicezero()
    {
        for (int i = 0; i < 5; i++)
        {
            m_dice[i] = 0;
        }
    }

    private void DiceButtonfunc(int idx)
    {
        if (m_dice[idx] <= 0)
            return;

        for (int i = 0; i < 5; i++)
        {
            if (DiceTray.instance.Trays(i).m_ani.gameObject.activeInHierarchy)
                continue;

            DiceTray.instance.Trays(i).m_diceidx = idx;
            DiceTray.instance.Trays(i).m_ani.gameObject.SetActive(true);
            DiceTray.instance.Trays(i).m_ani.SetInteger("dice", m_dice[idx]);
            m_buttons[idx].gameObject.SetActive(false);
            break;
        }
    }


    public void DiceShake()
    {

    }

    public void DiceRoll()
    {

        if (TurnMng.instance.Gameend)
            return;

        if (m_dicelock)
            return;


        int count = 0;
        m_dicelock = true;
        for (int i = 0; i < 5; i++)
        {
            if (!m_diceanis[i].gameObject.activeInHierarchy)
                continue;

            count++;
        }

        for (int i = 0; i < 5; i++)
        {
            if (!m_diceanis[i].gameObject.activeInHierarchy)
                continue;

            m_dice[i] = Random.Range(1, 7);

            m_diceanis[i].SetInteger("Diceroll", -1);

            StartCoroutine(IEdiceani(i, count));
        }

        if (count > 0)
            m_sound.gameObject.SetActive(true);

        m_rollcount--;
        LeftTexting(m_rollcount.ToString() + " Left");
     
    }


    private IEnumerator IEdiceani(int idx, int count)
    {
        float elapsedtime = 0;
        bool stop = false;
        m_dicerolling = true;
        while(!stop)
        {
            elapsedtime += Time.deltaTime;

            if (elapsedtime > 0.35f)
            {
                stop = true;
                m_diceanis[idx].SetInteger("Diceroll", m_dice[idx]);
                m_dicerolling = false;
                Points.instance.Check();
            }

            yield return null;
        }

        if (idx >= count - 1)
        {
            m_dicelock = false;
            m_sound.gameObject.SetActive(false);

            if (m_rollcount <= 0)
                m_dicelock = true;
            
        }

        yield return null;


    }

}
