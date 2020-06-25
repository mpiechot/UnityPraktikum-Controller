
public interface IState
{
    void Enter();
    void Execute();
    void Exit();
    bool finished {get;set;}
}
