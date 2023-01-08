using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _Instance;

    public AudioClip[] EatSounds;
    public AudioClip[] SpawnFoodSounds;
    public AudioClip DeathSting;
    public AudioClip[] DeathRattles;
    public AudioClip SickSound;
    public AudioClip[] SickRattles;
    public AudioClip[] HumanBarks;
    public AudioClip MaxArriveSound;
    public AudioClip MaxLeaveSound;
    public AudioClip[] MaxPainSound;
    public AudioClip HumanTeleportedAwaySound;
    public AudioClip MaxTeleportInSound;
    public AudioClip UiAlertSound;
    public AudioClip[] AttackSounds;

    [Space]
    [Header("UI")]
    public AudioClip UiFail;

    [Space]
    [Header("Audio Sources")]
    public GameObject CalmBgmGo;
    public GameObject CombatBgmGo;

    private void Awake()
    {
        _Instance = this;
    }

    public void CreateSoundAtPoint(AudioClip clip, Vector3 position, float volume)
    {
        GameObject soundGo = Instantiate(new GameObject(), position, Quaternion.identity);
        soundGo.name = clip.name;
        AudioSource source = soundGo.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0.3f;
        source.Play();

        Destroy(soundGo, clip.length);
    }

    public void CreateSoundGlobal(AudioClip clip, float volume)
    {
        GameObject soundGo = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        soundGo.name = clip.name;
        AudioSource source = soundGo.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0f;
        source.Play();

        Destroy(soundGo, clip.length);
    }

    public AudioClip RandomSoundFromArray(AudioClip[] clipArray)
    {
        return clipArray[Random.Range(0, clipArray.Length)];
    }
}
