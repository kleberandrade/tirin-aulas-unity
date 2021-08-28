using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FlockManager : MonoBehaviour
{
    [Header("Boids")]
    public GameObject m_Boid;
    public int m_BoidsNumber = 20;

    [Header("Settings")]
    public float m_TimeToChangeTarget = 5.0f;
    public float m_MinSpeed = 1.0f;
    public float m_MaxSpeed = 2.0f;
    public float m_NeighbourDistance = 2.0f;
    public float m_RotationSpeed = 20.0f;
    public float m_Distance = 1.0f;
    public float m_ViewDistance = 10.0f;
    [Range(0.0f, 1.0f)]
    public float m_SpeedRate = 0.1f;
    [Range(0.0f, 1.0f)]
    public float m_FlockRate = 0.25f;

    [Header("Area")]
    public Color m_AreaColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);
    public Color m_TargetColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);

    public BoxCollider Collider { get; private set; }
    public List<Flock> BoidsList { get; private set; }
    public Vector3 Target { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        Collider.isTrigger = true;
        BoidsList = new List<Flock>();
        for (int i = 0; i < m_BoidsNumber; i++)
        {
            var position = GetRandomPosition();
            var rotation = Quaternion.identity;
            var boid = Instantiate(m_Boid, position, rotation, transform);

            var flock = boid.GetComponent<Flock>();
            flock.m_Manager = this;

            BoidsList.Add(flock);
        }

        InvokeRepeating("UpdateTarget", 0.0f, m_TimeToChangeTarget);
    }

    private void UpdateTarget()
    {
        Target = GetRandomPosition();
    }

    private Vector3 GetRandomPosition()
    {
        var min = Collider.bounds.min;
        var max = Collider.bounds.max;
        var x = Random.Range(min.x, max.x);
        var y = Random.Range(min.y, max.y);
        var z = Random.Range(min.z, max.z);
        return new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        if (Collider == null) return;
        var center = Collider.bounds.center;
        var size = Collider.bounds.size;

        Gizmos.color = m_AreaColor;
        Gizmos.DrawCube(center, size);

        Gizmos.color = m_TargetColor;
        Gizmos.DrawSphere(Target, 0.5f);
    }
}
