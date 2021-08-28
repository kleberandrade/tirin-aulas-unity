using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager m_Manager;
    public float m_Speed;
    public Color m_NeighbourColor = new Color(0.0f, 0.0f, 1.0f, 0.05f);
    public Color m_SeparationColor = new Color(0.0f, 0.0f, 1.0f, 0.10f);
    private bool m_Turning;

    private void Start()
    {
        m_Speed = Random.Range(m_Manager.m_MinSpeed, m_Manager.m_MaxSpeed);
    }

    private Vector3 GetDirection()
    {
        var direction = Vector3.zero;
        if (!m_Manager.Collider.bounds.Contains(transform.position))
        {
            m_Turning = true;
            direction = m_Manager.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, transform.forward * m_Manager.m_ViewDistance, out RaycastHit hit))
        {
            m_Turning = true;
            direction = Vector3.Reflect(transform.forward, hit.normal);
        }
        else
        {
            m_Turning = false;
        }

        return direction;
    }

    private void Update()
    {
        var direction = GetDirection();
        Rotate(direction);
        Move();
    }

    private void Move()
    {
        var velocity = Vector3.forward * m_Speed * Time.deltaTime;
        transform.Translate(velocity, Space.Self);
    }

    private void Rotate(Vector3 direction)
    {
        if (m_Turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), m_Manager.m_RotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0.0f, 1.0f) < m_Manager.m_SpeedRate)
            {
                m_Speed = Random.Range(m_Manager.m_MinSpeed, m_Manager.m_MaxSpeed);
            }

            if (Random.Range(0.0f, 1.0f) < m_Manager.m_FlockRate)
            {
                ApplyFlockRules();
            }
        }
    }

    public void ApplyFlockRules()
    {
        int count = 0;
        float distance;
        float speed = 0.001f;
        Vector3 position = Vector3.zero;
        Vector3 separation = Vector3.zero;
        foreach (var boid in m_Manager.BoidsList)
        {
            if (boid == gameObject) continue;

            distance = Vector3.Distance(transform.position, boid.transform.position);
            if (distance > m_Manager.m_NeighbourDistance) continue;

            count++;
            position += boid.transform.position;
            speed += boid.m_Speed;

            if (distance < m_Manager.m_Distance)
            {
                separation = separation + (transform.position - boid.transform.position);
            }
        }

        if (count > 0)
        {
            position = position / count + (m_Manager.Target - transform.position);
            m_Speed = Mathf.Clamp(speed / count, m_Manager.m_MinSpeed, m_Manager.m_MaxSpeed);
            var direction = (position + separation) - transform.position;
            if (direction != Vector3.zero){
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), m_Manager.m_RotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = m_NeighbourColor;
        Gizmos.DrawSphere(transform.position, m_Manager.m_NeighbourDistance);

        Gizmos.color = m_SeparationColor;
        Gizmos.DrawSphere(transform.position, m_Manager.m_Distance);
    }
}
