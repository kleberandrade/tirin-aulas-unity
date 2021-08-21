using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public enum SteeringType { Seek, Flee, Wander, PathFollow, Hide }
    public SteeringType m_Type = SteeringType.Seek;

    private NavMeshAgent m_Agent;
    private GameObject m_Target;

    [Header("Flee")]
    public float m_SafeDistance = 10.0f;

    [Header("Wander")]
    public float m_WanderDistance = 5.0f;
    public float m_WanderTime = 1.0f;
    private float m_WanderNextDecision = 0.0f;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        m_Target = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        if (!m_Target) return;
        if (m_Type == SteeringType.Seek) Seek();
        if (m_Type == SteeringType.Flee) Flee();
        if (m_Type == SteeringType.Wander) Wander();

    }

    private void Seek()
    {
        m_Agent.SetDestination(m_Target.transform.position);
    }

    private void Flee()
    {
        if (Vector3.Distance(transform.position, m_Target.transform.position) > m_SafeDistance) return;
        var direction = (transform.position - m_Target.transform.position).normalized;
        var position = transform.position + direction * m_Agent.speed;
        m_Agent.SetDestination(position);
    }

    private void Wander()
    {
        if (Time.time < m_WanderNextDecision) return;
        m_WanderNextDecision += m_WanderTime;
        var position = GetRandomPosition();
        m_Agent.SetDestination(position);
    }

    private Vector3 GetRandomPosition()
    {
        var direction = Random.insideUnitSphere * m_WanderDistance;
        var position = transform.position + direction;
        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, m_WanderDistance, NavMesh.AllAreas);
        return hit.position;
    }

}
