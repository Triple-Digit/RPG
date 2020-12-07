using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverLifeTime : MonoBehaviour
{
    public float m_lifeTime;
    void Update()
    {
        Destroy(gameObject, m_lifeTime);
    }
}
