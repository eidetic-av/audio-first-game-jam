using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundBank : MonoBehaviour {

    public bool TriggerPlayback = false;

    [SerializeField] AudioMixerGroup Mixer;
    [SerializeField] List<AudioClip> Bank = new List<AudioClip>();
    [SerializeField] int StartIndex = -1;
    [SerializeField] bool PlayOnStart = false;
    [SerializeField] bool Loop = false;
    [SerializeField] Vector2 LoopDelay = new Vector2(0, 0);
    [SerializeField] bool RandomiseClipOnLoop = false;

    AudioSource audioSource;
    AudioSource AudioSource => audioSource ?? (audioSource = gameObject.GetComponent<AudioSource>());

    bool PlaybackActive = false;
    bool AwaitingLoop = false;
    float LoopWaitTime = -1f;

    public void Start() {
        if (Bank.Count == 0) return;

        // If StartIndex is not set, randomly pick an audio clip
        if (StartIndex == -1)
            StartIndex = Mathf.FloorToInt(UnityEngine.Random.Range(0, Bank.Count));

        // set up AudioSource
        AudioSource.outputAudioMixerGroup = Mixer;
        AudioSource.clip = Bank[StartIndex];
        AudioSource.spatialBlend = 1;
       
        if (PlayOnStart) Play();
    }

    public void Play(bool randomise = false) {
        if (AudioSource.isPlaying) AudioSource.Stop();
        if (randomise) {
            var clipIndex = Mathf.FloorToInt(UnityEngine.Random.Range(0, Bank.Count));
            AudioSource.clip = Bank[clipIndex];
        }
        AudioSource.Play();
        PlaybackActive = true;
    }

    public void Update() {
        if (TriggerPlayback) Play();

        if (PlaybackActive && !AudioSource.isPlaying) {
            if (!Loop) {
                PlaybackActive = false;
                return;
            }
            if (LoopDelay.x == 0 && LoopDelay.y == 0) {
                Play(RandomiseClipOnLoop);
                return;
            }
            if (!AwaitingLoop) {
                LoopWaitTime = UnityEngine.Random.Range(LoopDelay.x, LoopDelay.y);
                AwaitingLoop = true;
                return;
            }
            LoopWaitTime -= Time.deltaTime;
            if (LoopWaitTime <= 0) {
                Play(RandomiseClipOnLoop);
                AwaitingLoop = false;
            }
        }
    }

}
