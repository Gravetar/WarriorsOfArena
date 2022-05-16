public class StateMachine
{
    private State _currentState;

    public void Initialize (State startingState)
    {
        _currentState = startingState;
        startingState.Enter();
    }

    public void LogicUpdate()
    {
        _currentState?.LogicUpdate();
    }
    public void ChangeState(State newState)
    {
        _currentState.Exit();

        _currentState = newState;
        newState.Enter();
    }
}
