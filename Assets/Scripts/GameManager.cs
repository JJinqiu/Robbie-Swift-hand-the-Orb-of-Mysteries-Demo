using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private SceneFader m_Fader;
    private List<Orb> m_Orbs;
    private Door m_LockedDoor;

    private float m_GameTime;
    private bool m_IsGameOver;

    // public int orbCount;
    public int deathCount;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        m_Orbs = new List<Orb>();

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (m_IsGameOver) return;
        // orbCount = _instance.m_Orbs.Count;
        m_GameTime += Time.deltaTime;
        UIManager.UpdateTimeUI(m_GameTime);
    }

    public static void RegisterDoor(Door door)
    {
        _instance.m_LockedDoor = door;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (!_instance.m_Orbs.Contains(orb))
            _instance.m_Orbs.Add(orb);
        
        UIManager.UpdateOrbUI(_instance.m_Orbs.Count);
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!_instance.m_Orbs.Contains(orb))
            return;
        _instance.m_Orbs.Remove(orb);
        
        if (_instance.m_Orbs.Count == 0)
            _instance.m_LockedDoor.Open();
        UIManager.UpdateOrbUI(_instance.m_Orbs.Count);
    }

    public static void RegisterSceneFader(SceneFader obj)
    {
        _instance.m_Fader = obj;
    }

    public static void PlayerDied()
    {
        _instance.m_Fader.FadeOut();
        _instance.deathCount++;
        UIManager.UpdateDeathUI(_instance.deathCount);
        _instance.Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        _instance.m_Orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void PlayerWon()
    {
        _instance.m_IsGameOver = true;
        UIManager.DisplayGameOver();
    }

    public static bool GameOver()
    {
        return _instance.m_IsGameOver;
    }
}