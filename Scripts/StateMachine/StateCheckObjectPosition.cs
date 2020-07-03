using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCheckObjectPosition : MonoBehaviour, IState {
    
    public bool finished { get; set; }
    public IState next_state { get; set; }

    public GameObject state_check_action_peformed;
    public Collider starting_position;
    public Transform cylinder;

    public void Enter() {
        Debug.Log("Enter: StateCheckObjectPosition");
    }

    public void Execute() {
        if (starting_position.bounds.Contains(cylinder.position)) {
            Debug.Log("Object at original position");
            next_state = state_check_action_peformed.GetComponent<IState>();
            finished = true;
        }
    }

    public void Exit() {
        Debug.Log("Exit: StateCheckObjectPosition");
    }

}
