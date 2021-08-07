using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject m_Enemy;
    public Transform[] m_Points;
    private GameObject m_Target;

    private void Start()
    {
        m_Target = GameObject.FindGameObjectWithTag("Player");
    }

    public void Spawn()
    { 
        Vector3 position = NextRandomPosition();
        Quaternion rotation = TargetRotation(position);
        Instantiate(m_Enemy, position, rotation, transform);
    }

    public Vector3 NextRandomPosition()
    {
        int index = Random.Range(0, 10000) % m_Points.Length;
        return m_Points[index].position;
    }

    public Quaternion TargetRotation(Vector3 position)
    {
        Vector3 direction = m_Target.transform.position - position;
        return Quaternion.LookRotation(direction, Vector3.up);
    }
}
