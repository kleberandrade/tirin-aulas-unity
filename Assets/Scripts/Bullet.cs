using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float m_Speed = 500.0f;
    public float m_DestroyTime = 5.0f;

    private Rigidbody m_Body;

    private void Start()
    {
        m_Body = GetComponent<Rigidbody>();
        m_Body.velocity = transform.forward * m_Speed;
        Destroy(gameObject, m_DestroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
            var health = other.gameObject.GetComponent<Health>();
            health.TakeDamage(10.0f);
        }

        Destroy(gameObject);
    }
}
