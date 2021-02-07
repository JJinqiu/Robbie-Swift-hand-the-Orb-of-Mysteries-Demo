using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public TextMeshProUGUI orbText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI gameOverText;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(this);
    }

    public static void UpdateOrbUI(int orbCount)
    {
        _instance.orbText.text = orbCount.ToString();
    }

    public static void UpdateDeathUI(int deathCount)
    {
        _instance.deathText.text = deathCount.ToString();
    }

    public static void UpdateTimeUI(float time)
    {
        var minutes = (int) time / 60;
        var seconds = time % 60;

        _instance.timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    
    public static void DisplayGameOver()
    {
        _instance.gameOverText.enabled = true;
    } 
}