using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent m_Agent;
    private GameObject m_Target;

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

        m_Agent.SetDestination(m_Target.transform.position);
    }
}
