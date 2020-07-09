public class Experiment
{
    public PossibleFingerStimulations finger_stimulation { get; }
    public PossibleLightEffectSide light_effect_side { get; }
    public PossibleStimulationStart stimulation_start { get; }
    public PossibleObjectColor object_color { get; }
    public bool SuccessfulFinished { get; set; }

    public Experiment(PossibleFingerStimulations finger, PossibleLightEffectSide effect_side, PossibleStimulationStart start, PossibleObjectColor color)
    {
        this.finger_stimulation = finger;
        this.light_effect_side = effect_side;
        this.stimulation_start = start;
        this.object_color = color;
        this.SuccessfulFinished = false;
    }

    public override string ToString()
    {
        return "Experiment["+finger_stimulation + ", " + light_effect_side + ", " + stimulation_start + ", " + object_color + ", " + SuccessfulFinished + "]";
    }
}