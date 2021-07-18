using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFolder : MonoBehaviour
{
    private RectTransform m_transform;
    private FolderMng m_foldermng;
    private Vector3 m_originpos;
    private Vector3 m_targetpos;


    private bool m_open = false;

    public bool IsOpen
    {
        get { return m_open; }
    }


    public virtual void Init()
    {
        m_transform = GetComponent<RectTransform>();
        m_originpos = m_transform.localPosition;
        m_targetpos = transform.parent.Find("Pivot").transform.localPosition;
    }

    public virtual void MoveFolder()
    {

        Vector3 target = new Vector3();
        if(!m_open)
        {
            target = m_targetpos;
            m_open = true;
            transform.SetAsFirstSibling();
        }
        else
        {
            target = m_originpos;
            m_open = false;
        }
        StartCoroutine(IEMoveFolder(target));
       
    }

    private IEnumerator IEMoveFolder(Vector3 targetpos)
    {
        float elapsedtime = 0;
        Vector3 pos = m_transform.localPosition;
        bool stop = false;
        while (!stop)
        {
            elapsedtime += Time.deltaTime * 5;

            m_transform.localPosition = Vector3.Lerp(pos, targetpos, elapsedtime);
            if(elapsedtime >=1)
            {
                stop = true;
            }
            yield return null;

        }
        yield return null;

    }

}
