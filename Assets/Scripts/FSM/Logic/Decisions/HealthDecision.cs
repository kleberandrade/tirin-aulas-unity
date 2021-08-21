using UnityEngine;

[CreateAssetMenu(menuName = "FatecAI/Decisions/Health")]
public class HealthDecision : Decision
{
    public enum HealthLevel { None, Low, Medium, High }
    public enum HealthLogic { Less, Greather }

    public HealthLevel m_HealthLevel = HealthLevel.Low;
    public HealthLogic m_HealthLogic = HealthLogic.Less;

    public override bool Decide(StateController controller)
    {
        return Health(controller);
    }

    public bool Health(StateController controller)
    {
        if (m_HealthLogic == HealthLogic.Less)
            return CheckLessConditions(controller);
        else
            return CheckHighConditions(controller);
    }

    private bool CheckLessConditions(StateController controller)
    {
        if (m_HealthLevel == HealthLevel.Low) return HealthRate(controller.Health) <= 0.2f;
        if (m_HealthLevel == HealthLevel.Medium) return HealthRate(controller.Health) <= 0.5f;
        if (m_HealthLevel == HealthLevel.High) return HealthRate(controller.Health) <= 1.0f;
        return false;
    }

    private bool CheckHighConditions(StateController controller)
    {
        if (m_HealthLevel == HealthLevel.High) return HealthRate(controller.Health) >= 0.8f;
        if (m_HealthLevel == HealthLevel.Medium) return HealthRate(controller.Health) >= 0.5f;
        if (m_HealthLevel == HealthLevel.Low) return HealthRate(controller.Health) >= 0.0f;
        return false;
    }

    private float HealthRate(Health health)
    {
        return health.m_Energy / health.m_MaxEnergy;
    }
}
