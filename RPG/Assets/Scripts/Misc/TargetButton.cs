using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetButton : MonoBehaviour
{
    public string m_moveName;
    public int m_activeTarget;
    public Text m_target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        CombatManager.Instance.PlayerAttack(m_moveName, m_activeTarget);
    }
}
