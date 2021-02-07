using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private int m_Player;
    public GameObject explosionVFXPrefab;
    
    // Start is called before the first frame update
    private void Start()
    {
        m_Player = LayerMask.NameToLayer("Player");
        
        GameManager.RegisterOrb(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == m_Player)
        {
            Instantiate(explosionVFXPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlayOrbAudio();
            
            GameManager.PlayerGrabbedOrb(this);
        }
    }
}
