using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class StateWelcome : MonoBehaviour, IState {

    public bool finished { get; set; } // true if current state is finished and ready to call its Exit() method
    public IState next_state { get; set; } // follow-up state -> current_state transitions to next_state in Exit() method

    public GameObject next_object_go; // contains follow-up state

    public TextMeshProUGUI text; // text to display instructions to the users

    // for the visual representation of the states:
    public GameObject edgePrefab;
    private SpriteRenderer state_renderer;

    void Awake() {
        // initiates edges
        state_renderer = GetComponentInChildren<SpriteRenderer>();
        if (edgePrefab != null) {
            AddEdge(next_object_go.transform.position); 
        }
    }

    // draws edges between the visual representation of the states:
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
        state_renderer.material.color = Color.red; // active color of visual representation of state
        Debug.Log("Enter: StateWelcome");

        // instructions:
        text.text = "Hi wie gehts. Drücke n zum Starten!";

        // create a file with metadata
        DateTime t = DateTime.Now;
        string newFileName = t.Year + "-" + t.Month + "-" + t.Day + "_" + t.Hour + "-" + t.Minute + "-" + t.Second + ".csv";
        string clientHeader = "Fingerstimulation;LightEffectSide;SimulationStart;ObjectColor;SuccessfulFinished;ReactionTime;SpokenWord;HandPositions" + Environment.NewLine;
        InformationManager.filename = newFileName;
        File.WriteAllText(newFileName, clientHeader);
    }

    public void Execute() {
        // different output with different key input
        if (Input.GetKeyDown("space")) {
            text.text = "Du kannst die Leertaste drücken - super!";
        }
        // start experiment:
        if (Input.GetKeyDown("n")) {
            Debug.Log("Experiment about to start");
            next_state = next_object_go.GetComponent<IState>(); // prepare next state
            finished = true; // ready to exit state
        }
    }

    public void Exit() {
        state_renderer.material.color = Color.blue; // inactive color
        Debug.Log("Exit: StateWelcome");
        finished = false; // reset 
    }
}
