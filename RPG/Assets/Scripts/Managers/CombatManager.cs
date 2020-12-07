using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    #region Singleton
    public static CombatManager Instance { get; private set; }

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

    [Header("Scene Prefabs")]
    [SerializeField] bool m_combatActive;
    public GameObject m_combatScene;
    public GameObject m_uiButtons;
    public GameObject m_targetMenu;
    public TargetButton[] m_targetMenuButtons;
    public Text[] m_playerName, m_playerHealth;

    
    [Header("Combat Scene Character positions")]
    public Transform[] m_playerPositions;
    public Transform[] m_enemyPositions;

    [Header("Combat Character Stats")]
    public CombatStats[] m_playerPrefabs;
    public CombatStats[] m_enemyPrefabs;

    [Header("List of enemies")]
    public List<CombatStats> m_activeCharacters = new List<CombatStats>();

    [Header("Turn management")]
    public int m_currentTurn;
    public bool m_turnWaiting;
    public DamageText m_dmg;

    [Header("FXs")]
    public GameObject m_enemyAttackingFx;

    [Header("Attacks list")]
    public Combat[] m_attacks;

    private void Update()
    {
        TurnManagement();
    }


    public void StartCombat(string[] enemiesToSpawn)
    {
        if(!m_combatActive)
        {
            m_combatActive = true;
            PlayerController.Instance.m_canMove = false;
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            m_combatScene.SetActive(true);

            for(int i = 0; i< m_playerPositions.Length; i++)
            {
                if(GameManager.Instance.m_playerStats[i].gameObject)
                {
                    for(int j = 0; j < m_playerPrefabs.Length; j++)
                    {
                        if(m_playerPrefabs[j].m_characterName == GameManager.Instance.m_playerStats[i].m_characterName)
                        {
                            CombatStats newPlayer = Instantiate(m_playerPrefabs[j], m_playerPositions[i].position, m_playerPositions[i].rotation);
                            newPlayer.transform.parent = m_playerPositions[i];
                            m_activeCharacters.Add(newPlayer);
                            CombatStats playerStats = GameManager.Instance.m_playerStats[i];
                            m_activeCharacters[i].m_currentHealth = playerStats.m_currentHealth;
                            m_activeCharacters[i].m_maxHealth = playerStats.m_maxHealth;
                            m_activeCharacters[i].m_strenth = playerStats.m_strenth;                            
                        }
                    }
                }
            }
            for(int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if(enemiesToSpawn[i] != "")
                {
                    for(int j = 0; j < m_enemyPrefabs.Length; j++)
                    {
                        if(m_enemyPrefabs[j].m_characterName == enemiesToSpawn[i])
                        {
                            CombatStats newEnemy = Instantiate(m_enemyPrefabs[j], m_enemyPositions[i].position, m_enemyPositions[i].rotation);
                            newEnemy.transform.parent = m_enemyPositions[i];
                            m_activeCharacters.Add(newEnemy);
                        }                        
                    }
                }
            }
        }        
        m_turnWaiting = true;
        m_currentTurn = 0;
        UpdateUIStats();
    }

    public void TurnManagement()
    {
        if(m_combatActive)
        {
            if(m_turnWaiting)
            {
                if(m_activeCharacters[m_currentTurn].m_isPlayer)
                {
                    m_uiButtons.SetActive(true);
                }
                else
                {
                    m_uiButtons.SetActive(false);
                    StartCoroutine(EnemyCoroutine());
                }
            }
        }
    }

    public void NextTurn()
    {
        m_currentTurn++;
        if(m_currentTurn >= m_activeCharacters.Count)
        {
            m_currentTurn = 0; 
        }
        m_turnWaiting = true;
        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for(int i = 0; i < m_activeCharacters.Count; i++)
        {
            if(m_activeCharacters[i].m_currentHealth < 0)
            {
                m_activeCharacters[i].m_currentHealth = 0;
            }

            if(m_activeCharacters[i].m_currentHealth == 0)
            {
                if(m_activeCharacters[i].m_isPlayer)
                {
                    m_activeCharacters[i].m_activeSprite.sprite = m_activeCharacters[i].m_skullSprite;
                }
                else
                {
                    m_activeCharacters[i].m_hasDied = true;
                }
            }
            else
            {
                if(m_activeCharacters[i].m_isPlayer)
                {
                    allPlayersDead = false;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }
        if(allEnemiesDead || allPlayersDead)
        {
            if(allEnemiesDead)
            {
                StartCoroutine(EndCombat());
            }
            else
            {
                StartCoroutine(GameOver());
            }            
        }
        else
        {
            while(m_activeCharacters[m_currentTurn].m_currentHealth == 0)
            {
                m_currentTurn++;
                if(m_currentTurn >= m_activeCharacters.Count)
                {
                    m_currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyCoroutine()
    {
        m_turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();

        for(int i =0; i < m_activeCharacters.Count; i++)
        {
            if(m_activeCharacters[i].m_isPlayer && m_activeCharacters[i].m_currentHealth > 0)
            {
                players.Add(i);
            }
        }

        int selectTarget = players[Random.Range(0, players.Count)];

        int selectAttack = Random.Range(0, m_activeCharacters[m_currentTurn].m_movesAvailable.Length);
        int combatPower = 0;
        for(int i =0; i < m_attacks.Length; i++)
        {
            if(m_attacks[i].m_attackName == m_activeCharacters[m_currentTurn].m_movesAvailable[selectAttack])
            {
                Instantiate(m_attacks[i].m_fx, m_activeCharacters[selectTarget].transform.position, m_activeCharacters[selectTarget].transform.rotation);
                combatPower = m_attacks[i].m_attackPower;
            }
        }

        Instantiate(m_enemyAttackingFx, m_activeCharacters[m_currentTurn].transform.position, m_activeCharacters[m_currentTurn].transform.rotation);
        DealDamage(selectTarget, combatPower);
    }

    public void DealDamage(int target, int attackPower)
    {
        float strengthOfAttack = m_activeCharacters[m_currentTurn].m_strenth;
        float defenceOfOpponent = m_activeCharacters[target].m_defense;

        float totaldamage = (strengthOfAttack / defenceOfOpponent) * attackPower * Random.Range(0.9f, 1.1f);
        int damage = Mathf.RoundToInt(totaldamage);

        m_activeCharacters[target].m_currentHealth -= damage;
        Instantiate(m_dmg, m_activeCharacters[target].transform.position, m_activeCharacters[target].transform.rotation).SetDamage(damage);
        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for(int i = 0; i <  m_playerName.Length; i++)
        {
            if(m_activeCharacters.Count > i)
            {
                if (m_activeCharacters[i].m_isPlayer)
                {
                    CombatStats playerData = m_activeCharacters[i];
                    m_playerName[i].gameObject.SetActive(true);
                    m_playerName[i].text = playerData.m_characterName;
                    m_playerHealth[i].text = Mathf.Clamp(playerData.m_currentHealth, 0, int.MaxValue) + "/" + playerData.m_maxHealth;
                }
                else
                {
                    m_playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                m_playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string attackName, int selectedTarget)
    {
        int combatPower = 0;
        for (int i = 0; i < m_attacks.Length; i++)
        {
            if (m_attacks[i].m_attackName == attackName)
            {
                Instantiate(m_attacks[i].m_fx, m_activeCharacters[selectedTarget].transform.position, m_activeCharacters[selectedTarget].transform.rotation);
                combatPower = m_attacks[i].m_attackPower;
            }            
        }
        Instantiate(m_enemyAttackingFx, m_activeCharacters[m_currentTurn].transform.position, m_activeCharacters[m_currentTurn].transform.rotation);
        DealDamage(selectedTarget, combatPower);
        m_uiButtons.SetActive(false);
        m_targetMenu.SetActive(false);
        NextTurn();
    }

    public void OpenTargetMenu(string attackName)
    {
        m_targetMenu.SetActive(true);
        List<int> enemies = new List<int>();
        for(int i = 0; i < m_activeCharacters.Count; i++)
        {
            if(!m_activeCharacters[i].m_isPlayer)
            {
                enemies.Add(i);
            }
        }

        for(int i = 0; i < m_targetMenuButtons.Length; i++)
        {
            if(enemies.Count > i && m_activeCharacters[enemies[i]].m_currentHealth > 0)
            {
                m_targetMenuButtons[i].gameObject.SetActive(true);
                m_targetMenuButtons[i].m_moveName = attackName;
                m_targetMenuButtons[i].m_activeTarget = enemies[i];
                m_targetMenuButtons[i].m_target.text = m_activeCharacters[enemies[i]].m_characterName;
            }
            else
            {
                m_targetMenuButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator EndCombat()
    {
        m_uiButtons.SetActive(false);
        m_targetMenu.SetActive(false);
        

        yield return new WaitForSeconds(.5f);
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < m_activeCharacters.Count; i++)
        {
            if(m_activeCharacters[i].m_isPlayer)
            {
                for(int j = 0; j < GameManager.Instance.m_playerStats.Length; j++)
                {
                    if(m_activeCharacters[i].m_characterName == GameManager.Instance.m_playerStats[j].m_characterName)
                    {
                        GameManager.Instance.m_playerStats[j].m_currentHealth = m_activeCharacters[i].m_currentHealth;
                    }
                }
            }
            Destroy(m_activeCharacters[i].gameObject);
        }

        UIFade.Instance.FadeFromBlack();
        m_combatScene.SetActive(false);
        m_activeCharacters.Clear();
        m_currentTurn = 0;
        GameManager.Instance.m_combatActive = false;
    }

    public IEnumerator GameOver()
    {
        GameManager.Instance.m_combatActive = false;
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.Lose();
    }
    
}
