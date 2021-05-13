using UnityEngine;

public class Test_BeatManager : MonoBehaviour
{

    public static Test_BeatManager instance;

    public float songBpm;
    public float secPerBeat;
    public float songPosition;
    public float songPositionInBeats;
    public float dspSongTime;

    public AudioSource musicSource;

    public float beatsPerLoop;
    public int completedLoops = 0;
    public float loopPositionInBeats;
    public float noteNumber;
    public int numberOfAllowedLoops;

    public float songBpmAfterPitch;
    public float secPerBeatAfterPitch;

    public bool ShouldPlay = false;
    public bool isPlaying = false;
    public bool loopsAllowed = false;

    private void Awake()
    {
        instance = this;

        musicSource = GetComponent<AudioSource>();

        secPerBeat = 60f / songBpm;

        dspSongTime = (float)AudioSettings.dspTime;
        
        songBpmAfterPitch = songBpm * musicSource.pitch;

        secPerBeatAfterPitch = 60f / songBpmAfterPitch;
        
        beatsPerLoop = musicSource.clip.length / musicSource.pitch / secPerBeatAfterPitch;
    }

    void Update()
    {
        musicSource.loop = loopsAllowed;

        if(completedLoops >= numberOfAllowedLoops)
        {
            ShouldPlay = false;
            musicSource.Stop();
            completedLoops = 0;
            isPlaying = false;
        }

        if (ShouldPlay)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                dspSongTime = (float)AudioSettings.dspTime;
                musicSource.Play();
            }
        }
        else
        {
            return;
        }

        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        songPositionInBeats = songPosition / secPerBeatAfterPitch;

        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;

        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;

        noteNumber = Mathf.Round(loopPositionInBeats % 4) - Mathf.Round(loopPositionInBeats %1);
    }
}
