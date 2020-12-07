using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    bool m_exited;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!m_exited && collision.tag == "Player")
        {
            GameManager.Instance.Win();
            m_exited = true;
        }
    }
}
