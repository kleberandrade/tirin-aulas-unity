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

    [Header("Way Points")]
    public Transform[] m_Points;
    public float m_Accuracy = 0.5f;
    private int m_CurrentPoint = 0;

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
        if (m_Type == SteeringType.PathFollow) PathFollow();
        if (m_Type == SteeringType.Hide) Hide();
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

    private void PathFollow()
    {
        var target = m_Points[m_CurrentPoint];
        m_Agent.SetDestination(target.position);
        var distance = Vector3.Distance(transform.position, target.position);
        var accuracy = m_Agent.stoppingDistance + m_Accuracy;
        if (distance > accuracy) return;
        m_CurrentPoint = ++m_CurrentPoint % m_Points.Length;
    }

    [Header("Hide")]
    private GameObject[] m_Obstacles;
    public float m_ObstacleRadius = 3.0f;   

    private void FindObstacles()
    {
        m_Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    private List<Vector3> CalculateHidePositions()
    {
        var positions = new List<Vector3>();
        foreach (var obstacle in m_Obstacles)
        {
            var direction = (obstacle.transform.position - m_Target.transform.position).normalized;
            var position = obstacle.transform.position + direction * m_ObstacleRadius;
            positions.Add(position);
        }

        return positions;
    }

    public void Hide()
    {
        if (m_Obstacles == null) FindObstacles();
        var positions = CalculateHidePositions();
        var position = positions.OrderBy(p => Vector3.Distance(p, transform.position)).First();
        m_Agent.SetDestination(position);
    }
}
