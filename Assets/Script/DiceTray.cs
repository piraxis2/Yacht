using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tray
{

    public int m_diceidx = -1;
    public Button m_button;
    public Animator m_ani;
    public Image m_image;
    public int m_idx = -1;

}



public class DiceTray : MonoBehaviour
{

    private static DiceTray s_dicetray;
    public static DiceTray instance
    {
        get
        {
            if(s_dicetray == null)
            {
                s_dicetray = FindObjectOfType<DiceTray>();
                s_dicetray.init();
            }
            return s_dicetray;
        }

    }

    private List<Button> m_buttons = new List<Button>();
    private List<Animator> m_anis = new List<Animator>();
    private Tray[] m_trays = new Tray[5];

    public Tray Trays(int idx)
    {
        if (idx > 4)
            return null;

        return m_trays[idx];
    }




    private void init()
    {
        m_buttons.AddRange(GetComponentsInChildren<Button>(true));
        m_anis.AddRange(GetComponentsInChildren<Animator>(true));



        for (int i = 0; i < 5; i++)
        {
            m_trays[i] = new Tray();
            m_trays[i].m_idx = i;
            m_trays[i].m_button = m_buttons[i];
            m_trays[i].m_ani = m_anis[i];
            int idx = i;
            m_trays[i].m_button.onClick.AddListener(() => { Buttonfunc(idx); });
        }

    }

    private void Buttonfunc(int idx)
    {
        m_trays[idx].m_ani.gameObject.SetActive(false);
        Dice.instance.Ani(m_trays[idx].m_diceidx).gameObject.SetActive(true);
        Dice.instance.Ani(m_trays[idx].m_diceidx).SetInteger("Diceroll", Dice.instance.dice(m_trays[idx].m_diceidx));
        m_trays[idx].m_diceidx = -1;
    }

}
