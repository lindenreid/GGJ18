using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class PlayAnimation : MonoBehaviour
{
    public AnimationClip clip;
    PlayableGraph playableGraph;
    public bool playAuto;

    void Start ()
    {
        if (playAuto) Play(false);
    }

    public void Play (bool randomStart)
    {
        Initialize(randomStart);
        playableGraph.Play();
    }

    public void PlayClip(AnimationClip c, bool randomStart)
    {
        if (playableGraph.IsValid()) playableGraph.Destroy();
        clip = c;
        Play(randomStart);
    }

    private void Initialize (bool randomStart)
    {
        playableGraph = PlayableGraph.Create();
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        Animator animator = GetComponent<Animator>();
        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);

        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        if (randomStart) 
            clipPlayable.SetTime(Random.Range(0.0f, 1.0f));
        else 
            clipPlayable.SetTime(0.0f);
        playableOutput.SetSourcePlayable(clipPlayable);
    }

    void OnDisable()
    {
        if (playableGraph.IsValid()) playableGraph.Destroy();
    }
}