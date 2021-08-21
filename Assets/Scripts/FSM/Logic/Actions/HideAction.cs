using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "FatecAI/Actions/Hide")]
public class HideAction : Action
{
    public float m_ObstacleRadius = 3.0f;

    public override void Execute(StateController controller)
    {
        Hide(controller);
    }

    public void Hide(StateController controller)
    {
        var positions = CalculateHidePositions(controller);
        var position = positions.OrderBy(p => Vector3.Distance(p, controller.Position)).First();
        controller.Agent.SetDestination(position);
    }

    private List<Vector3> CalculateHidePositions(StateController controller)
    {
        var positions = new List<Vector3>();
        foreach (var obstacle in controller.Obstacles)
        {
            var direction = (obstacle.transform.position - controller.Player.transform.position).normalized;
            var position = obstacle.transform.position + direction * m_ObstacleRadius;
            positions.Add(position);
        }

        return positions;
    }
}
