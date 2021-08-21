using System;

[Serializable]
public class Transition
{
    public Decision m_Decision;
    public State m_CurrentState;
    public State m_NextState;
}

