using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator m_Animator;
    private int m_OpenID;
    
    // Start is called before the first frame update
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_OpenID = Animator.StringToHash("Open");
        // 注册门
        GameManager.RegisterDoor(this);
    }

    public void Open()
    {
        m_Animator.SetTrigger(m_OpenID);
        // 播放音乐
        AudioManager.PlayDoorOpenAudio();
    }
}
