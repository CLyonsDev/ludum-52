using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SourceState {
    FadeOut,
    Done,
    FadeIn
}

public class BgmManager : MonoBehaviour
{
    public SourceState sourceState;
    private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (sourceState)
        {
            case SourceState.FadeOut:
            source.volume =  Mathf.Lerp(source.volume, 0f, 1.5f * Time.deltaTime);
            if(source.volume <= 0.005f)
            {
                source.volume = 0f;
                sourceState = SourceState.Done;
            }
            break;
            case SourceState.Done:
            break;
            case SourceState.FadeIn:
            source.volume =  Mathf.Lerp(source.volume, 0.04f, 1.5f * Time.deltaTime);
            if(source.volume >= 0.035f)
            {
                source.volume = 0.04f;
                sourceState = SourceState.Done;
            }
            break;
        }
    }

    public void FadeOut()
    {
        sourceState = SourceState.FadeOut;
    }

    public void FadeIn()
    {
        sourceState = SourceState.FadeIn;
    }
}
