using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    public GameObject deathGasPrefab;
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
            Instantiate(deathGasPrefab, transform.position, transform.rotation);
            Instantiate(deathVFXPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(-45, 90)));
            gameObject.SetActive(false);
            AudioManager.PlayDeathAudio();

            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            GameManager.PlayerDied();
        }
    }
}