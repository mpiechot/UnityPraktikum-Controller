using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCheckActionPerformed : MonoBehaviour, IState {
    
    public bool finished { get; set; }
    public IState next_state { get; set; }

    public GameObject state_goodbye;
    public Collider target_position;
    public Transform cylinder;

    public void Enter() {
        Debug.Log("Enter: StateActionPerformed");
    }

    public void Execute() {
        // TODO: zusätzlich abfragen, bzgl verbaler Reaktion
        if (target_position.bounds.Contains(cylinder.position)) {
            Debug.Log("Object at target position");
            Debug.Log("TODO: Verbale Reaktion fehlt noch");
            next_state = state_goodbye.GetComponent<IState>();
            finished = true;
        }
        
    }

    public void Exit() {
        Debug.Log("Exit: StateActionPerformed");
    }

}
