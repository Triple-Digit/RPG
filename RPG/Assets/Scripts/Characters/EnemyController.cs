using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public float m_movementSpeed = 3;
    Rigidbody2D m_body;

    public float m_distanceToChasePlayer;
    Vector3 m_moveDirection;

    private void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < m_distanceToChasePlayer)
        {
            m_moveDirection = PlayerController.Instance.transform.position - transform.position;
        }
        m_moveDirection.Normalize();

        m_body.velocity = m_moveDirection * m_movementSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            m_movementSpeed = 0;
        }

    }
}
