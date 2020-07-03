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

    private Experiment[] experiments;
    private int currentExperiment = 0;
    private MeshRenderer state_renderer;

    void Awake()
    {
        state_renderer = GetComponentInChildren<MeshRenderer>();
        if (edgePrefab != null)
        {
            GameObject newEdge = Instantiate(edgePrefab);
            Edge arrow = newEdge.GetComponent<Edge>();
            arrow.start = this.transform.position;
            arrow.target = checkTargetPositionState.transform.position;
        }
    }

    public void Enter()
    {
        state_renderer.material.color = Color.red;
        if(experiments == null)
        {
            Init();
            currentExperiment = -1;
        }
    }

    public void Execute()
    {
        state_renderer.material.color = Color.green;
        currentExperiment++;
        if (currentExperiment < experiments.Length)
        {
            Debug.Log("Count: " + currentExperiment + ": " + experiments[currentExperiment]);
            InformationManager.actual_experiment = experiments[currentExperiment];
            next_state = checkTargetPositionState.GetComponent<IState>();
        }
        else
        {
            //Experiments finished
            next_state = doneState.GetComponent<IState>();
        }
        finished = true;
        state_renderer.material.color = Color.red;
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

        experiments = new Experiment[stimulations.Length * light_effects.Length * stimulation_starts.Length * object_colors.Length * numberOfRuns];
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
                            experiments[i++] = new Experiment(stimulation, light_effect, stimulation_start, object_color);
                        }
                    }
                }
            }
        }
        Shuffle();
    }
    //TODO Maybe another shuffle algorihm? This one seems to be not that great :/
    private void Shuffle()
    {
        for (int i = 0; i < experiments.Length - 1; i++)
        {
            int rnd = Random.Range(i, experiments.Length);
            var tempGO = experiments[rnd];
            experiments[rnd] = experiments[i];
            experiments[i] = tempGO;
        }
    }
}