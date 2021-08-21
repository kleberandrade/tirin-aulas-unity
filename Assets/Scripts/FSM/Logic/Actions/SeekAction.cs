using UnityEngine;

[CreateAssetMenu(menuName = "FatecAI/Actions/Seek")]
public class SeekAction : Action
{
    public override void Execute(StateController controller)
    {
        Seek(controller);
    }

    public void Seek(StateController controller)
    {
        var position = controller.Player.transform.position;
        controller.Agent.SetDestination(position);
    }
}

