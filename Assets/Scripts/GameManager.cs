
using UnityEngine;

[RequireComponent(typeof(SpawnEnemy))]
public class GameManager : MonoBehaviour
{
    public int m_MaxEnemies = 1;
    public float m_MinTimeToSpawn = 2.0f;
    public float m_MaxTimeToSpawn = 4.0f;

    private SpawnEnemy m_Spawner;
    private float m_NextTimeToSpawn = 0.0f;

    private void Awake()
    {
        m_Spawner = GetComponent<SpawnEnemy>();
    }

    private void LateUpdate()
    {
        if (Time.time < m_NextTimeToSpawn) return;
        if (transform.childCount >= m_MaxEnemies) return;

        m_Spawner.Spawn();
        UpdateNextTimeToSpawn();
    }

    private void UpdateNextTimeToSpawn()
    {
        float time = Random.Range(m_MinTimeToSpawn, m_MaxTimeToSpawn);
        m_NextTimeToSpawn += time;
    }
}
