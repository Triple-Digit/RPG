using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFX : MonoBehaviour
{
    public float m_fxLength;

    void Update()
    {
        Destroy(gameObject, m_fxLength);
    }
}
