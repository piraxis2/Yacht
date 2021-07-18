using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FolderMng : MonoBehaviour
{

    private static FolderMng m_foldermng;

    public static FolderMng Instance
    {
        get
        {
            if (m_foldermng ==null)
            {
                m_foldermng = GameObject.Find("Main/Canvas/Folders").GetComponent<FolderMng>();
                m_foldermng.Init();
            }

            return m_foldermng;

        }
    }


    public Vector3 m_target = new Vector3();
    private UIFolder[] m_folder = new UIFolder[3];
    public List<Button> m_buttons;





    private void Init()
    {
        m_target = transform.GetChild(0).transform.localPosition;
        m_folder = GetComponentsInChildren<UIFolder>();
        foreach(var x in m_folder)
        {
            x.Init();
            m_buttons.Add(x.GetComponentInChildren<Button>());
        }

    }

    public void Awake()
    {
        FolderMng x = Instance;
    }

    public void FoldRest()
    {
        foreach(var x in m_folder)
        {
            if(x.IsOpen)
            {
                x.MoveFolder();
            }
        }
    }

    void Update()
    {
        
        //if (!EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        FoldRest();
        //    }
        //}



    }

  
}
