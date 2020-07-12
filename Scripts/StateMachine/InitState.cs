using Array = System.Array;
using Enum = System.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : MonoBehaviour,IState
{
    public int numberOfRuns = 10;
    public GameObject checkTargetPositionState;
    public GameObject doneState;
    public GameObject edgePrefab;

    public bool finished {get;set;}
    public IState next_state{get;set;}

    private List<Experiment> experiments = new List<Experiment>();
    private int currentExperimentID = 0;
    private SpriteRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<SpriteRenderer>();
        state_renderer.material.color = Color.blue;

        if (edgePrefab != null)
        {
            AddEdge(checkTargetPositionState.transform.position);
            AddEdge(doneState.transform.position);
        }
    }
    void AddEdge(Vector3 target)
    {
        GameObject newEdge = Instantiate(edgePrefab, transform);
        LineRendererArrow arrow = newEdge.GetComponent<LineRendererArrow>();
        arrow.ArrowOrigin = this.transform.position;
        arrow.ArrowTarget = target;
    }
    public void Enter()
    {
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
        if (current_experiment != null && !current_experiment.SuccessfulFinished)
        {
            experiments.Add(current_experiment);
        }
        currentExperimentID++;
        if (currentExperimentID < experiments.Count)
        {
            InformationManager.actual_experiment = experiments[currentExperimentID];
            Debug.Log("Count: " + currentExperimentID + ": " + experiments[currentExperimentID]);
            next_state = checkTargetPositionState.GetComponent<IState>();
        }
        else
        {
            //Experiments finished
            next_state = doneState.GetComponent<IState>();
        }
        finished = true;
        return;
    }

    public void Exit()
    {
        state_renderer.material.color = Color.blue;
        return;
    }

    private void Init()
    {
        Array stimulations = Enum.GetValues(typeof(PossibleFingerStimulations));
        Array light_effects = Enum.GetValues(typeof(PossibleLightEffectSide));
        Array stimulation_starts = Enum.GetValues(typeof(PossibleStimulationStart));
        Array object_colors = Enum.GetValues(typeof(PossibleObjectColor));

        int i = 0;
        foreach (PossibleFingerStimulations stimulation in stimulations)
        {
            foreach (PossibleLightEffectSide light_effect in light_effects)
            {
                foreach (PossibleStimulationStart stimulation_start in stimulation_starts)
                {
                    foreach (PossibleObjectColor object_color in object_colors)
                    {
                        for (int run = 0; run < numberOfRuns; run++)
                        {
                            experiments.Add(new Experiment(stimulation, light_effect, stimulation_start, object_color));
                        }
                    }
                }
            }
        }
        Shuffle();
        //foreach(Experiment e in experiments)
        //{
        //    Debug.Log(e);
        //}
    }
    //TODO Maybe another shuffle algorihm? This one seems to be not that great :/
    //TODO Link zur Quelle, Fisher-Yates 
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