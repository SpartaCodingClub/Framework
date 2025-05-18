using DG.Tweening;
using System;

public class SequenceHandler
{
    public Sequence Birth { get; private set; }
    public Sequence Stand { get; private set; }
    public Sequence Death { get; private set; }

    public void Initialize()
    {
        Birth = Utility.RecyclableSequence();
        Stand = Utility.RecyclableSequence().SetLoops(-1);
        Death = Utility.RecyclableSequence();
    }

    public void Deinitialize()
    {
        Birth.Kill();
        Stand.Kill();
        Death.Kill();
    }

    public void Pause()
    {
        Birth.Pause();
        Stand.Pause();
        Death.Pause();
    }

    public void BindSequences(State type, params Func<Sequence>[] sequences)
    {
        Sequence sequence = sequences[0]();
        for (int i = 1; i < sequences.Length; i++)
        {
            sequence.Insert(0.0f, sequences[i]());
        }

        switch (type)
        {
            case State.Birth:
                Birth.Append(sequence);
                break;
            case State.Stand:
                Stand.Append(sequence);
                break;
            case State.Death:
                Death.Append(sequence);
                break;
            default:
                Debug.LogWarning($"{Define.FAILED_TO_}{nameof(BindSequences)}({type})");
                break;
        }
    }
}