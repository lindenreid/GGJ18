using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class PlayAnimation : MonoBehaviour
{
    public AnimationClip clip;
    PlayableGraph playableGraph;

    void Start()
    {
        if (!playableGraph.IsValid()) Initialize();
    }

    public void Play ()
    {
        if (!playableGraph.IsValid()) Initialize();
        playableGraph.Play();
    }

    public void PlayClip(AnimationClip c)
    {
        if (playableGraph.IsValid()) playableGraph.Destroy();
        clip = c;
        Initialize();
        Play();
    }

    private void Initialize ()
    {
        playableGraph = PlayableGraph.Create();
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        Animator animator = GetComponent<Animator>();
        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);

        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        clipPlayable.SetTime(Random.Range(0.0f, 1.0f));
        playableOutput.SetSourcePlayable(clipPlayable);
    }

    void OnDisable()
    {
        playableGraph.Destroy();
    }
}