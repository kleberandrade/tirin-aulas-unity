using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class StateController : MonoBehaviour
{
    public State m_CurrentState;
    public Transform m_Eye;
    public State m_RamainState;

    public NavMeshAgent Agent { get; set; }
    public float StateTimeElapsed { get; set; }

    public List<Transform> WayPoints { get; set; }
    public int CurrentWayPoint { get; set; }
    public Health Health { get; set; }

    public Vector3 Position => transform.position;
    public GameObject Player { get; set; }
    public GameObject[] Obstacles { get; set; }

    public bool m_Enabled = true;

    private void Awake()
    {
        Health = GetComponent<Health>();
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    private void Update()
    {
        if (!m_Enabled) return;
        m_CurrentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != m_RamainState)
        {
            m_CurrentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        StateTimeElapsed += Time.deltaTime;
        return StateTimeElapsed >= duration;
    }

    private void OnExitState()
    {
        StateTimeElapsed = 0.0f;
    }

    private void OnDrawGizmos()
    {
        if (m_CurrentState != null && m_Eye != null)
        {
            Gizmos.color = m_CurrentState.m_StateColor;
            Gizmos.DrawSphere(m_Eye.position, 0.5f);
        }
    }
}
