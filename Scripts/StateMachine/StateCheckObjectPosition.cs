using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateCheckObjectPosition : MonoBehaviour, IState {
    
    public bool finished { get; set; }
    public IState next_state { get; set; }

    public GameObject next_state_go;
    public Collider starting_position;
    public Transform cylinder;

    public GameObject edgePrefab;

    public TextMeshProUGUI text;

    public int duration;
    private Coroutine coroutine;
    private bool coroutine_running = false;

    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(next_state_go.transform.position);
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
        if(state_renderer == null){
            state_renderer = GetComponentInChildren<SpriteRenderer>();
        }
        state_renderer.material.color = Color.red;
        Debug.Log("Enter: StateCheckObjectPosition");
        text.text = "Stelle den Zylinder in die Startposition";
    }

    public void Execute() {
        if (starting_position.bounds.Contains(cylinder.position)) {
            if (!coroutine_running) {
                coroutine = StartCoroutine(CheckPositionOverTime(duration));
            }
        } else {
            if(coroutine_running){
                text.text = "Stelle den Zylinder in die Startposition!";
                StopCoroutine(coroutine);
                coroutine_running = false;
            }
        }
    }

    public void Exit() {
        state_renderer.material.color = Color.blue;
        Debug.Log("Exit: StateCheckObjectPosition");
        coroutine_running = false;
        finished = false;
    }

    public IEnumerator CheckPositionOverTime(int object_in_position) {
        coroutine_running = true;
        for(int i = object_in_position; i > 0; i--){
            text.text = "Warten..." + i;
            yield return new WaitForSeconds(1);
        }
        text.text = "Fertig";
        next_state = next_state_go.GetComponent<IState>();
        finished = true;
    }

}
