using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instance { get; private set; }

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

        m_body = GetComponent<Rigidbody2D>();
        m_canMove = true;

    }
    #endregion

    public float m_movementSpeed = 5;
    Rigidbody2D m_body;
    public bool m_canMove;

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!m_canMove) return;
        m_body.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * m_movementSpeed;
    }
}
