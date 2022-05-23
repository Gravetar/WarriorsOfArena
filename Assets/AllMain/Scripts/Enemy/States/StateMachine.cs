public class StateMachine
{
    public State CurrentState;

    public void Initialize (State startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void LogicUpdate()
    {
        CurrentState?.LogicUpdate();
    }
    public void ChangeState(State newState)
    {
        CurrentState.Exit();

        CurrentState = newState;
        newState.Enter();
    }
}
