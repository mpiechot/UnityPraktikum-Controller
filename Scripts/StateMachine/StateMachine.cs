using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public GameObject currentStateGameObject;
    IState currentState;

    public static bool trialPhase = true; 
    public GameObject s;
    
    void Start(){
        if(currentStateGameObject != null){
            currentState = currentStateGameObject.GetComponent<IState>();
            currentState.Enter();
        }
    }

    void Update()
    {
        
        if (currentState != null) {
            currentState.Execute();
            if(currentState.finished){
                ChangeState();
            }
        }

        if(trialPhase){
            if(Input.GetKeyDown(KeyCode.X)){
                trialPhase = false;
                currentState.Exit();

                Debug.Log("Jetzt wirds ernst");
                GameObject[] visuals = GameObject.FindGameObjectsWithTag("NonTrial"); 
                for(int i = 0; i < visuals.Length; i++){
                    visuals[i].GetComponent<SpriteRenderer>().color = new Color(0,0,255,1);
                }
                visuals = GameObject.FindGameObjectsWithTag("Trial"); 
                for(int i = 0; i < visuals.Length; i++){
                    visuals[i].GetComponent<SpriteRenderer>().color = new Color(0,0,255,0);
                }

                currentState = s.GetComponent<IState>();
                currentState.Enter();
            }
        }
    }


    public void ChangeState()
    {
        if(currentState != null){
            currentState.Exit();
            currentState = currentState.next_state;
            currentState.Enter();
        }
    }
    
}
