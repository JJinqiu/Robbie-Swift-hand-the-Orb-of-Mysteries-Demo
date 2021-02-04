using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D m_Rb2d;
    private BoxCollider2D m_Collider;

    [Header("移动参数")] public float speed = 8f;
    public float crouchSpeedDivisor = 3f;

    [Header("跳跃参数")] public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;

    private float m_JumpTime;

    [Header("状态")] public bool isCrouch;
    public bool isOnGround;
    public bool isJump;

    [Header("环境检测")] public LayerMask groundLayer;

    private float m_XVelocity;

    // 按键设置
    private bool m_JumpPressed;
    private bool m_JumpHeld;
    private bool m_CrouchHeld;

    // 碰撞体尺寸
    private Vector2 m_ColliderStandSize;
    private Vector2 m_ColliderStandOffset;
    private Vector2 m_ColliderCrouchSize;
    private Vector2 m_ColliderCrouchOffset;

    // Start is called before the first frame update
    private void Start()
    {
        m_Rb2d = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<BoxCollider2D>();

        m_ColliderStandSize = m_Collider.size;
        m_ColliderStandOffset = m_Collider.offset;
        m_ColliderCrouchSize = new Vector2(m_Collider.size.x, m_Collider.size.y / 2);
        m_ColliderCrouchOffset = new Vector2(m_Collider.offset.x, m_Collider.offset.y / 2);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_JumpPressed = true;
        }

        m_JumpHeld = Input.GetButton("Jump");
        m_CrouchHeld = Input.GetButton("Crouch");
    }

    private void FixedUpdate()
    {
        if (!Input.GetButton("Jump"))
        {
            m_JumpPressed = false;
        }

        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
    }

    private void PhysicsCheck()
    {
        if (m_Collider.IsTouchingLayers(groundLayer))
            isOnGround = true;
        else isOnGround = false;
    }

    private void GroundMovement()
    {
        if (m_CrouchHeld && !isCrouch && isOnGround)
            Crouch();
        else if (!m_CrouchHeld && isCrouch)
            StandUp();
        else if (!isOnGround && isCrouch) 
            StandUp();

        m_XVelocity = Input.GetAxis("Horizontal"); // -1f~1f
        if (isCrouch)
            m_XVelocity /= crouchSpeedDivisor;

        m_Rb2d.velocity = new Vector2(m_XVelocity * speed, m_Rb2d.velocity.y);
        FlipDirection();
    }

    private void MidAirMovement()
    {
        if (m_JumpPressed && isOnGround && !isJump)
        {
            if (isOnGround && isCrouch)
            {
                StandUp();
                m_Rb2d.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            
            isOnGround = false;
            isJump = true;

            m_JumpTime = Time.time + jumpHoldDuration;

            m_Rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        else if (isJump)
        {
            if (m_JumpHeld)
                m_Rb2d.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            if (m_JumpTime < Time.time)
                isJump = false;
        }
    }

    private void FlipDirection()
    {
        if (m_XVelocity < 0)
            transform.localScale = new Vector2(-1, 1);
        if (m_XVelocity > 0)
            transform.localScale = new Vector2(1, 1);
    }

    private void Crouch()
    {
        isCrouch = true;
        m_Collider.size = m_ColliderCrouchSize;
        m_Collider.offset = m_ColliderCrouchOffset;
    }

    private void StandUp()
    {
        isCrouch = false;
        m_Collider.size = m_ColliderStandSize;
        m_Collider.offset = m_ColliderStandOffset;
    }
}