using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    private int m_TrapsLayer;
    
    // Start is called before the first frame update
    private void Start()
    {
        m_TrapsLayer = LayerMask.NameToLayer("Traps");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == m_TrapsLayer)
        {
            Instantiate(deathVFXPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlayDeathAudio();

            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
            GameManager.PlayerDied();
        }
    }
}
