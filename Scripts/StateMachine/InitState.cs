using Array = System.Array;
using Enum = System.Enum;
using System.Collections.Generic;
using UnityEngine;

public class InitState : MonoBehaviour,IState
{
    //Visualization
    private SpriteRenderer state_renderer;

    public GameObject edgePrefab;

    //StateMachine
    public GameObject checkTargetPositionState;
    public GameObject doneState;
    public bool finished {get;set;}
    public IState next_state{get;set;}

    //Experiments
    private List<Experiment> experiments = new List<Experiment>();
    private int currentExperimentID = 0;

    void Awake()
    {
        //Visualize StateMachine
        state_renderer = GetComponentInChildren<SpriteRenderer>();

        if (edgePrefab != null)
        {
            AddEdge(checkTargetPositionState.transform.position);
            AddEdge(doneState.transform.position);
        }
    }
    //Draw an Edge using the LineRenderer from this state to its targetState
    void AddEdge(Vector3 target)
    {
        GameObject newEdge = Instantiate(edgePrefab, transform);
        LineRendererArrow arrow = newEdge.GetComponent<LineRendererArrow>();
        arrow.ArrowOrigin = this.transform.position;
        arrow.ArrowTarget = target;
    }
    public void Enter()
    {
        if(state_renderer == null){
            state_renderer = GetComponentInChildren<SpriteRenderer>();
        }
        state_renderer.material.color = Color.red;
        StopAllCoroutines();
        if(experiments.Count == 0)
        {
            Init();
            currentExperimentID = -1;
        }
    }

    public void Execute()
    {
        Experiment current_experiment = InformationManager.actual_experiment;
        //Check if the last experiment was successufll, if not -> add this setup to the list again.
        if (current_experiment != null && !current_experiment.SuccessfulFinished)
        {
            current_experiment = new Experiment(current_experiment.finger_stimulation,current_experiment.light_effect_side,current_experiment.stimulation_start, current_experiment.object_color);
            experiments.Add(current_experiment);
        }
        //Select the next Experiment Setup
        currentExperimentID++;
        if (currentExperimentID < experiments.Count)
        {
            InformationManager.actual_experiment = experiments[currentExperimentID];
            Debug.Log("Experiment Number: " + currentExperimentID);
            Debug.Log("Setup: " + experiments[currentExperimentID]);
            next_state = checkTargetPositionState.GetComponent<IState>();
        }
        else
        {
            //All Experiments finished
            next_state = doneState.GetComponent<IState>();
        }
        finished = true;
        return;
    }

    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        finished = false;
        return;
    }

    private void Init()
    {
        Array stimulations = Enum.GetValues(typeof(PossibleFingerStimulations));
        Array light_effects = Enum.GetValues(typeof(PossibleLightEffectSide));
        Array stimulation_starts = Enum.GetValues(typeof(PossibleStimulationStart));
        Array object_colors = Enum.GetValues(typeof(PossibleObjectColor));

        foreach (PossibleFingerStimulations stimulation in stimulations)
        {
            foreach (PossibleLightEffectSide light_effect in light_effects)
            {
                foreach (PossibleStimulationStart stimulation_start in stimulation_starts)
                {
                    foreach (PossibleObjectColor object_color in object_colors)
                    {
                        for (int run = 0; run < StateMachine.numberOfRuns; run++)
                        {
                            experiments.Add(new Experiment(stimulation, light_effect, stimulation_start, object_color));
                        }
                    }
                }
            }
        }
        Shuffle();
    }
    //Source: https://answers.unity.com/questions/1189736/im-trying-to-shuffle-an-arrays-order.html
    private void Shuffle()
    {
        for (int i = 0; i < experiments.Count - 1; i++)
        {
            int rnd = Random.Range(i, experiments.Count);
            var tempGO = experiments[rnd];
            experiments[rnd] = experiments[i];
            experiments[i] = tempGO;
        }
    }
}