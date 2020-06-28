﻿

public class StateMachine
{
    IState currentState;

    public void SetState(IState state){
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState()
    {
        currentState.Exit();
        currentState = currentState.next_state;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null) currentState.Execute();
    }

    public bool isStateFinished(){
        return currentState.finished;
    }

    
}
