using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;
    private PlayerMovement m_Movement;
    private Rigidbody2D m_Rb2d;

    private int m_GroundID;
    private int m_HangingID;
    private int m_CrouchID;
    private int m_SpeedID;
    private int m_FallID;

    // Start is called before the first frame update
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Movement = GetComponentInParent<PlayerMovement>();
        m_Rb2d = GetComponentInParent<Rigidbody2D>();
        m_GroundID = Animator.StringToHash("isOnGround");
        m_HangingID = Animator.StringToHash("isHanging");
        m_CrouchID = Animator.StringToHash("isCrouching");
        m_SpeedID = Animator.StringToHash("speed");
        m_FallID = Animator.StringToHash("verticalVelocity");

    }

    // Update is called once per frame
    private void Update()
    {
        m_Animator.SetFloat(m_SpeedID, Mathf.Abs(m_Movement.xVelocity));
        m_Animator.SetBool(m_GroundID, m_Movement.isOnGround);
        m_Animator.SetBool(m_HangingID, m_Movement.isHanging);
        m_Animator.SetBool(m_CrouchID, m_Movement.isCrouch);
        m_Animator.SetFloat(m_FallID, m_Rb2d.velocity.y);
    }

    public void StepAudio()
    {
        AudioManager.PlayFootStepAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchFootStepAudio();
    }
}