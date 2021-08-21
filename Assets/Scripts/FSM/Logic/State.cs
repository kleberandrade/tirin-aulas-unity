using UnityEngine;

[CreateAssetMenu (menuName = "FatecAI/State")]
public class State : ScriptableObject
{
    public Action[] m_Actions;
    public Transition[] m_Transitions;
    public Color m_StateColor = Color.grey;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    public void DoActions(StateController controller)
    {
        foreach (var action in m_Actions)
        {
            action.Execute(controller);
        }
    }

    public void CheckTransitions(StateController controller)
    {
        foreach (var transition in m_Transitions)
        {
            bool succeeded = transition.m_Decision.Decide(controller);
            if (succeeded)
            {
                controller.TransitionToState(transition.m_NextState);
            }
            else
            {
                controller.TransitionToState(transition.m_CurrentState);
            }
        }
    }
}
