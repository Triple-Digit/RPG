using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    public BattleType[] m_battleTypes;
    bool m_startBattle;    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !m_startBattle)
        {
            m_startBattle = true;
            StartCoroutine(StartBattle());
        }        
    }

    public IEnumerator StartBattle()
    {
        
        GameManager.Instance.m_combatActive = true;
        int selectedBattle = Random.Range(0, m_battleTypes.Length);
        yield return new WaitForSeconds(.5f);
        StartBattle(selectedBattle);
        gameObject.SetActive(false);
    }

    private void StartBattle(int selectedBattle)
    {        
        CombatManager.Instance.StartCombat(m_battleTypes[selectedBattle].m_enemies);
        
    }
}
