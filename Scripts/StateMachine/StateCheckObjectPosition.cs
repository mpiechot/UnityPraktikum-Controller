using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This state checks if the cylinder is in the initial position. 

public class StateCheckObjectPosition : MonoBehaviour, IState {
    
    public bool finished { get; set; } // true if current state is finished and ready to call its Exit() method
    public IState next_state { get; set; } // follow-up state -> current_state transitions to next_state in Exit() method
    
    public GameObject next_state_go; // contains follow-up state
    
    public TextMeshProUGUI text; // text to display instructions to the users

    public Collider starting_position; // starting position of the cylinder 
    public Transform cylinder;

    public int duration; // necessary time (in seconds) the cylinder must remain at the starting position 
    // coroutine is handling the countdown 
    private Coroutine coroutine;
    private bool coroutine_running = false;

    // for the visual representation of the states:
    public GameObject edgePrefab;
    private SpriteRenderer state_renderer;

    public float epsilon = 10; // valid range (+/- epsilon) for rotation of cylinder to reduce the noise of the measurement device


    void Awake() {
        // initiates edges
        state_renderer = GetComponentInChildren<SpriteRenderer>();
        if (edgePrefab != null) {
            AddEdge(next_state_go.transform.position);
        }
    }

    // draws edges between the visual representation of the states:
    void AddEdge(Vector3 target) {
        GameObject newEdge = Instantiate(edgePrefab, transform);
        LineRendererArrow arrow = newEdge.GetComponent<LineRendererArrow>();
        arrow.ArrowOrigin = this.transform.position;
        arrow.ArrowTarget = target;
    }

    // Updates visual state representation and gives instruction to the user.
    public void Enter() {
        if(state_renderer == null) {
            state_renderer = GetComponentInChildren<SpriteRenderer>();
        }
        state_renderer.material.color = Color.red; // active color of visual representation of state
        
        Debug.Log("Enter: StateCheckObjectPosition");
        // instructions:
        text.text = "Stelle den Zylinder in die Startposition";
    }

    // Checks if the cylinder is at the starting position. In this case, a coroutine is
    // started counting down. If the cylinder is moved before the coroutine is finished
    // the coroutine is canceled.
    public void Execute() {
        // if object is at start position and the coroutine has not yet been started, start coroutine
        if (starting_position.bounds.Contains(cylinder.position)) {
            if (Mathf.Abs(180 - cylinder.transform.rotation.eulerAngles.z) <= epsilon) {
                text.text = "Drehe den Zylinder um 180 Grad!";
            } else if (!coroutine_running) {
                coroutine = StartCoroutine(CheckPositionOverTime(duration));
            }
        // if object is not at start position and the coroutine is already running, stop coroutine and give user new instructions
        } else {
            if(coroutine_running){
                text.text = "Stelle den Zylinder in die Startposition!";
                StopCoroutine(coroutine);
                coroutine_running = false;
            }
        }
    }

    // Reset variables, prepare transit into next state.
    public void Exit() {
        state_renderer.material.color = Color.blue; // inactive color of visual representation of state
        Debug.Log("Exit: StateCheckObjectPosition");
        coroutine_running = false;
        finished = false;
    }

    // Handling the countdown (duration):
    // When the countdown is over, prepare the transitition to the exit method
    public IEnumerator CheckPositionOverTime(int object_in_position) {
        coroutine_running = true;
        for(int i = object_in_position; i > 0; i--){
            text.text = "Warten..." + i; // inform user about countdown
            yield return new WaitForSeconds(1);
        }
        text.text = "Fertig";
        next_state = next_state_go.GetComponent<IState>();
        finished = true;
    }
}
