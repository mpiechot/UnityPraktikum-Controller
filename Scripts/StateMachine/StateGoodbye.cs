using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateGoodbye : MonoBehaviour, IState {

    public bool finished { get; set; } // true if current state is finished and ready to call its Exit() method
    public IState next_state { get; set; } // follow-up state -> current_state transitions to next_state in Exit() method

    public TextMeshProUGUI text; // instructions for user

    // for the visual representation of the states:
    public GameObject edgePrefab;
    private SpriteRenderer state_renderer;

    void Awake() {
        state_renderer = GetComponentInChildren<SpriteRenderer>();
        if (edgePrefab != null) {
            
        }
    }

    void AddEdge(Vector3 target) {
        GameObject newEdge = Instantiate(edgePrefab, transform);
        LineRendererArrow arrow = newEdge.GetComponent<LineRendererArrow>();
        arrow.ArrowOrigin = this.transform.position;
        arrow.ArrowTarget = target;
    }

    public void Enter() {
        if(state_renderer == null){
            state_renderer = GetComponentInChildren<SpriteRenderer>();
        }
        state_renderer.material.color = Color.red;
        Debug.Log("Enter: StateGoodbye");
        text.text = "Danke fürs Mitmachen.";
        
    }

    public void Execute() {
        if (Input.GetKeyDown("space")) {
            text.text = "Hat sicher viel Spaß gemacht oder?";
        }
    }

    public void Exit() {
        state_renderer.material.color = Color.blue;
        Debug.Log("Exit: StateGoodbye");
        finished = false;
    }

}
