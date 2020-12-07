using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    #region Singleton
    public static UIFade Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
    
    public Image m_fadeScreen;
    public float m_fadeSpeed = 1f;
    bool m_fadeToBlack, m_fadeOutFromBlack;
    
    void Update()
    {
        if(m_fadeToBlack)
        {
            m_fadeScreen.color = new Color(m_fadeScreen.color.r, m_fadeScreen.color.g, m_fadeScreen.color.b, Mathf.MoveTowards(m_fadeScreen.color.a, 1f, m_fadeSpeed * Time.deltaTime));
            if(m_fadeScreen.color.a == 1f)
            {
                m_fadeToBlack = false;
            }
        }
        if(m_fadeOutFromBlack)
        {
            m_fadeScreen.color = new Color(m_fadeScreen.color.r, m_fadeScreen.color.g, m_fadeScreen.color.b, Mathf.MoveTowards(m_fadeScreen.color.a, 0f, m_fadeSpeed * Time.deltaTime));
            if (m_fadeScreen.color.a == 0f)
            {
                m_fadeOutFromBlack = false;
            }
        }
    }

    public void FadeToBlack()
    {
        m_fadeToBlack = true;
        m_fadeOutFromBlack = false;
    }

    public void FadeFromBlack()
    {
        m_fadeOutFromBlack = true;
        m_fadeToBlack = false;
    }
}
