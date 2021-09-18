using Panda;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PandaBehaviour))]
[RequireComponent(typeof(Weapon))]
public class PlayerBT : MonoBehaviour
{
    public float m_Radius = 5.0f;
    public LayerMask m_EnemyLayer;
    public float m_WanderDistance = 10.0f;
    public float m_LowEnergyRate = 0.2f;
    public float m_RotationSpeed = 10.0f;
    public float m_AngleAccuracy = 5.0f;

    private Health m_Health;
    private Weapon m_Weapon;
    private NavMeshAgent m_Agent;

    private void Start()
    {
        m_Health = GetComponent<Health>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Weapon = GetComponent<Weapon>();
    }

    [Task]
    public void LookAtPlayer()
    {
        var target = GetPlayerNearPosition();
        var direction = target - transform.position;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * m_RotationSpeed
        );

        var angle = Vector3.Angle(transform.forward, direction);
        if (Task.isInspected)
            Task.current.debugInfo = $"Angle = {angle}";

        if (angle < m_AngleAccuracy)
            Task.current.Succeed();
    }

    [Task]
    public void Fire()
    {
        if (m_Weapon.CanFire)
        {
            m_Weapon.Fire();
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }   
    }

    [Task]
    public void SetDestination()
    {
        var position = GetRandomPosition();
        m_Agent.SetDestination(position);
        Task.current.Succeed();
    }

    [Task]
    public void MoveToDestination()
    {
        if (Task.isInspected)
            Task.current.debugInfo = $"t={Time.time:0.00}";

        if (m_Agent.remainingDistance <= m_Agent.stoppingDistance && !m_Agent.pathPending)
            Task.current.Succeed();
    }

    private Vector3 GetRandomPosition()
    {
        var direction = Random.insideUnitSphere * m_WanderDistance;
        var position = transform.position + direction;
        NavMesh.SamplePosition(position, out NavMeshHit hit, m_WanderDistance, NavMesh.AllAreas);
        return hit.position;
    }

    [Task]
    public void Flee()
    {
        var enemy = GetPlayerNearPosition();
        if (Vector3.Distance(transform.position, enemy) > m_Radius)
        {
            Task.current.Succeed();
        }

        var direction = (transform.position - enemy).normalized;
        var position = transform.position + direction * m_Agent.speed;
        m_Agent.SetDestination(position);
    }

    public Vector3 GetPlayerNearPosition()
    {
        var colliders = Physics.OverlapSphere(transform.position, m_Radius, m_EnemyLayer);
        var enemies = colliders.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position)).ToList();
        enemies.RemoveAt(0);
        return enemies.First().transform.position;
    }

    [Task]
    public bool IsLowEnergy()
    {
        return (m_Health.m_Energy / m_Health.m_MaxEnergy) < m_LowEnergyRate;
    }

    [Task]
    public bool IsPlayerNear()
    {
        var colliders = Physics.OverlapSphere(transform.position, m_Radius, m_EnemyLayer);
        return colliders.Length > 1;
    }

    [Task]
    public bool IsHealthy()
    {
        return m_Health.HaveEnergy;
    }

    [Task]
    public void Die()
    {
        Destroy(gameObject);
        Task.current.Succeed();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.05f);
        Gizmos.DrawSphere(transform.position, m_Radius);
    }
}
