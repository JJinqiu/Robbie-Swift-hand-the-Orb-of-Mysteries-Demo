using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    private Animator m_Animator;
    private int m_FaderID;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_FaderID = Animator.StringToHash("Fade");
        GameManager.RegisterSceneFader(this);
    }

    public void FadeOut()
    {
        m_Animator.SetTrigger(m_FaderID);
    }
}
