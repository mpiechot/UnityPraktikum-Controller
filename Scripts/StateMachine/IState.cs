using UnityEngine;


public interface IState
{
    void Enter();
    void Execute();
    void Exit();
    bool finished {get;set;}
    IState next_state{get;set;}
}
