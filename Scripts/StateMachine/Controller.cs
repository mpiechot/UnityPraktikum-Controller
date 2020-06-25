using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    StateMachine stateMachine = new StateMachine();
   
    void Start()
    {
        //stateMachine.ChangeState(new TestState(this));
    }
 
    void Update()
    {
        //stateMachine.Update();
    }

}
