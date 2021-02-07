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

    public int orbCount;
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
        orbCount = _instance.m_Orbs.Count;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (!_instance.m_Orbs.Contains(orb))
            _instance.m_Orbs.Add(orb);
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!_instance.m_Orbs.Contains(orb))
            return;
        _instance.m_Orbs.Remove(orb);
    }

    public static void RegisterSceneFader(SceneFader obj)
    {
        _instance.m_Fader = obj;
    }

    public static void PlayerDied()
    {
        _instance.m_Fader.FadeOut();
        _instance.deathCount++;
        _instance.Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        _instance.m_Orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}