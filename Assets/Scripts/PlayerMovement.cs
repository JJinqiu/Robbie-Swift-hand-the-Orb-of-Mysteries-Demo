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

    [Header("状态")] public bool isCrouch;

    private float m_XVelocity;

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
    }

    private void FixedUpdate()
    {
        GroundMovement();
    }

    private void GroundMovement()
    {
        if (Input.GetButton("Crouch"))
            Crouch();
        else if (!Input.GetButton("Crouch") && isCrouch)
            StandUp();

        m_XVelocity = Input.GetAxis("Horizontal"); // -1f~1f
        if (isCrouch)
            m_XVelocity /= crouchSpeedDivisor;
        
        m_Rb2d.velocity = new Vector2(m_XVelocity * speed, m_Rb2d.velocity.y);
        FlipDirection();
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