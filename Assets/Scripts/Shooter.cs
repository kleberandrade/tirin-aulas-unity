using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform m_FirePoint;
    public Transform m_Bullet;
    public float m_CooldownTime = 0.2f;

    private float m_NextFireTime = 0.0f;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= m_NextFireTime)
        {
            Fire();
        }
    }

    public void Fire()
    {
        Instantiate(m_Bullet, m_FirePoint.position, m_FirePoint.rotation);
        m_NextFireTime += m_CooldownTime;
    }
}
