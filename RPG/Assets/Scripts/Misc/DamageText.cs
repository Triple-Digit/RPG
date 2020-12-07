using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text m_damageText;
    public float m_lifeTime = 1f;
    public float m_moveSpeed = 1f;
    public float m_placementRandomizer = .5f;
        
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, m_lifeTime);
        transform.position += new Vector3(0f, m_moveSpeed * Time.deltaTime, 0f);
    }

    public void SetDamage(int damageAmount)
    {
        m_damageText.text = damageAmount.ToString();
        transform.position += new Vector3(Random.Range(-m_placementRandomizer, m_placementRandomizer), Random.Range(-m_placementRandomizer, m_placementRandomizer), 0f);
    }
}
