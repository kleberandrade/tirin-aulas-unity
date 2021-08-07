using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float m_WalkSpeed = 4.0f;
    public float m_AngularSpeed = 180.0f;

    private Vector3 m_Movement = Vector3.zero;
    private Rigidbody m_Body;

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Control();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Control()
    {
        m_Movement.x = Input.GetAxis("Horizontal");
        m_Movement.y = 0.0f;  
        m_Movement.z = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        Vector3 displacement = m_Movement.normalized * m_WalkSpeed * Time.fixedDeltaTime;
        m_Body.MovePosition(m_Body.position + displacement);
    }

    private void Rotate()
    {
        Vector3 position = GetMouseInWorldPosition();
        Vector3 direction = position - transform.position;
        m_Body.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private Vector3 GetMouseInWorldPosition()
    {
        float offset = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 depth = new Vector3(0, 0, offset);
        Vector3 mousePosition = Input.mousePosition + depth;
        Vector3 position = Camera.main.ScreenToWorldPoint(mousePosition);
        position.y = transform.position.y;
        return position;
    }
}
