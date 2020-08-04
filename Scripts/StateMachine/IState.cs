using UnityEngine;

// Interface, which defines the methods for each state

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
    bool finished {get;set;}
    IState next_state{get;set;}
}
