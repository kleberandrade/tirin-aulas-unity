using UnityEngine;

[CreateAssetMenu (menuName = "FatecAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public float m_Accuracy = 2.0f;

    public override void Execute(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        var target = controller.WayPoints[controller.CurrentWayPoint];
        controller.Agent.SetDestination(target.position);

        var distance = Vector3.Distance(controller.Position, target.position);
        var accuracy = controller.Agent.stoppingDistance + m_Accuracy;
        if (distance > accuracy) return;

        controller.CurrentWayPoint = ++controller.CurrentWayPoint % controller.WayPoints.Count;
    }
}
