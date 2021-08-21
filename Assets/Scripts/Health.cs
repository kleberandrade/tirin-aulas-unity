using UnityEngine;

public class Health : MonoBehaviour
{
    public float m_Energy = 100.0f;
    public float m_MaxEnergy = 120.0f;

    public bool HaveEnergy => m_Energy > 0.0f;

    public void TakeDamage(float value)
    {
        SetLife(-value, 0.0f, m_MaxEnergy);
        Debug.Log($"Damage: {value} >>> {m_Energy}/{m_MaxEnergy}");
    }

    public void Recover(float value)
    {
        SetLife(value, 0.0f, m_MaxEnergy);
        Debug.Log($"Recover: {value} >>> {m_Energy}/{m_MaxEnergy}");
    }

    private void SetLife(float value, float min, float max)
    { 
        m_Energy = Mathf.Clamp(m_Energy + value, min, max);
    }
}
