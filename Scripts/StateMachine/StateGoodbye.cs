using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateGoodbye : MonoBehaviour, IState {

    public bool finished { get; set; }
    public IState next_state { get; set; }

    public TextMeshProUGUI text;
    public GameObject edgePrefab;

    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            
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
        Debug.Log("Enter: StateGoodbye");
        text.text = "Enter Goodbye State";
        
    }

    public void Execute() {
        if (Input.GetKeyDown("space")) {
            text.text = "Danke fürs Mitmachen.";
        }
    }

    public void Exit() {
        state_renderer.material.color = Color.blue;
        Debug.Log("Exit: StateGoodbye");
        finished = false;
    }

}
