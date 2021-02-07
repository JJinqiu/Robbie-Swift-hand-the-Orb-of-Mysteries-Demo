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
    public float hangingJumpForce = 15;

    private float m_JumpTime;

    [Header("状态")] public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlocked;
    public bool isHanging;

    [Header("环境检测")] public float footOffset = 0.4f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;
    private float m_PlayerHeight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;

    public LayerMask groundLayer;

    public float xVelocity;

    // 按键设置
    private bool m_JumpPressed;
    private bool m_JumpHeld;
    private bool m_CrouchHeld;
    private bool m_CrouchPressed;

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

        m_PlayerHeight = m_Collider.size.y;
        m_ColliderStandSize = m_Collider.size;
        m_ColliderStandOffset = m_Collider.offset;
        m_ColliderCrouchSize = new Vector2(m_Collider.size.x, m_Collider.size.y / 2);
        m_ColliderCrouchOffset = new Vector2(m_Collider.offset.x, m_Collider.offset.y / 2);
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.GameOver()) return;
        if (Input.GetButtonDown("Jump"))
        {
            m_JumpPressed = true;
        }

        m_JumpHeld = Input.GetButton("Jump");
        m_CrouchHeld = Input.GetButton("Crouch");
        m_CrouchPressed = Input.GetButtonDown("Crouch");
    }

    private void FixedUpdate()
    {
        if (GameManager.GameOver())
        {
            m_Rb2d.velocity = new Vector2(0, 0);
            return;
        }
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
        // 左右脚射线
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        if (leftCheck || rightCheck)
            isOnGround = true;
        else isOnGround = false;

        // 头顶射线
        RaycastHit2D headCheck = Raycast(new Vector2(0f, m_Collider.size.y), Vector2.up, headClearance, groundLayer);
        isHeadBlocked = headCheck;

        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, m_PlayerHeight), grabDir, grabDistance,
            groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance,
            groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, m_PlayerHeight), Vector2.down,
            grabDistance, groundLayer);

        if (!isOnGround && m_Rb2d.velocity.y < 0 && ledgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;

            pos.x += (wallCheck.distance - 0.05f) * direction;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;

            m_Rb2d.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }

    private void GroundMovement()
    {
        if (isHanging) return;
        if (m_CrouchHeld && !isCrouch && isOnGround)
            Crouch();
        else if (!m_CrouchHeld && isCrouch && !isHeadBlocked)
            StandUp();
        else if (!isOnGround && isCrouch)
            StandUp();

        xVelocity = Input.GetAxis("Horizontal"); // -1f~1f
        if (isCrouch)
            xVelocity /= crouchSpeedDivisor;

        m_Rb2d.velocity = new Vector2(xVelocity * speed, m_Rb2d.velocity.y);
        FlipDirection();
    }

    private void MidAirMovement()
    {
        if (isHanging)
        {
            if (m_JumpPressed)
            {
                m_Rb2d.bodyType = RigidbodyType2D.Dynamic;
                m_Rb2d.velocity = new Vector2(m_Rb2d.velocity.x, hangingJumpForce);
                isHanging = false;
            }

            if (m_CrouchPressed)
            {
                m_Rb2d.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }

        if (m_JumpPressed && isOnGround && !isJump && !isHeadBlocked)
        {
            if (isCrouch)
            {
                StandUp();
                m_Rb2d.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }

            isOnGround = false;
            isJump = true;

            m_JumpTime = Time.time + jumpHoldDuration;

            m_Rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            AudioManager.PlayJumpAudio();
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
        if (xVelocity < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (xVelocity > 0)
            transform.localScale = new Vector3(1, 1, 1);
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

    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        var hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        var color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }
}