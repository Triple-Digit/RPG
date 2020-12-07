using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStats : MonoBehaviour
{
    public bool m_isPlayer;
    public string[] m_movesAvailable;

    public string m_characterName;
    public int m_currentHealth, m_maxHealth, m_strenth, m_defense;
    public bool m_hasDied;

    public SpriteRenderer m_activeSprite;
    public Sprite m_skullSprite, m_characterSprite;
    public float m_fadeSpeed = 1f;

    private void Update()
    {
        if(m_hasDied && !m_isPlayer)
        {
            m_activeSprite.color = new Color(m_activeSprite.color.r, m_activeSprite.color.g, m_activeSprite.color.b, Mathf.MoveTowards(m_activeSprite.color.a, 0f, m_fadeSpeed * Time.deltaTime));
            if(m_activeSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
