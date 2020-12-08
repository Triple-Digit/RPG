using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    #region Singleton
    public static RoomTemplate Instance { get; private set; }

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

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;
    public List<GameObject> rooms;

    public float m_waitTime;
    private bool m_spawnExit;
    public GameObject m_exit;

    public GameObject[] m_enemies;
    public bool m_spawnedEnemies;
    public int m_numberOfEnemiesToSpawn;

    private void Update()
    {
        SpawnExit();
        SpawnEnemy();
    }

    public void SpawnExit()
    {
        if (m_waitTime <= 0 && m_spawnExit == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(m_exit, rooms[i].transform.position, Quaternion.identity);
                    m_spawnExit = true;
                }
            }
        }
        else
        {
            m_waitTime -= Time.deltaTime;
        }
    }

    public void SpawnEnemy()
    {
        if (!m_spawnExit) return;
        
        if (m_numberOfEnemiesToSpawn > 0f)
        {
            Instantiate(m_enemies[Random.Range(0, m_enemies.Length)], rooms[Random.Range(1, rooms.Count - 1)].transform.position, Quaternion.identity);
            m_numberOfEnemiesToSpawn--;
            
            if (m_numberOfEnemiesToSpawn <= 0f)
            {
                m_spawnedEnemies = true;
            }
        }        
    }
}
