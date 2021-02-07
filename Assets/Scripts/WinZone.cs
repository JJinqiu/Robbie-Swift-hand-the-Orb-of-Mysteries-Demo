using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private int m_PlayerLayer;
    
    // Start is called before the first frame update
    private void Start()
    {
        m_PlayerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == m_PlayerLayer)
        {
            Debug.Log("Player Won!");
            // 游戏结束
            GameManager.PlayerWon();
        }
    }
}
