using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateWelcome : MonoBehaviour, IState {

    public bool finished { get; set; }
    public IState next_state { get; set; }

    public TextMeshProUGUI text;
    public GameObject next_object_go;

    public GameObject edgePrefab;

    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();
        state_renderer.material.color = Color.blue;

        if (edgePrefab != null)
        {
            AddEdge(next_object_go.transform.position);
        }
    }
    void AddEdge(Vector3 target)
    {
        GameObject newEdge = Instantiate(edgePrefab, transform);
        LineRendererArrow arrow = newEdge.GetComponent<LineRendererArrow>();
        arrow.ArrowOrigin = this.transform.position;
        arrow.ArrowTarget = target;
    }

    public void Enter() {
        state_renderer.material.color = Color.red;
        Debug.Log("Enter: StateWelcome");
        text.text = "Hi wie gehts. Drücke n zum Starten!";
    }

    public void Execute() {
        if (Input.GetKeyDown("space")) {
            text.text = "Du kannst die Leertaste drücken - super!";
        }
        if (Input.GetKeyDown("n")) {
            Debug.Log("Experiment about to start");
            next_state = next_object_go.GetComponent<IState>();
            finished = true;
        }
    }

    public void Exit() {
        state_renderer.material.color = Color.blue;
        Debug.Log("Exit: StateWelcome");
        finished = false;
    }
}
