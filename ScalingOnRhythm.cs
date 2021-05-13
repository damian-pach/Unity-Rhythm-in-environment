using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingOnRhythm : MonoBehaviour
{
    private float currentTime;
    private int numOfBeats = 0;
    public float lowerTreshold, higherTreshold;
    public float beatOffset = 0.1f;
    public float scaleAmount = 1.5f;

    void Update()
    {
        if (currentTime / Test_BeatManager.instance.secPerBeatAfterPitch >= 1)
        {
            numOfBeats += 1;
        }

        currentTime = Test_BeatManager.instance.songPosition - numOfBeats * Test_BeatManager.instance.secPerBeatAfterPitch;
        currentTime %= Test_BeatManager.instance.secPerBeatAfterPitch;

        float t = currentTime / Test_BeatManager.instance.secPerBeatAfterPitch;
        t = t * t * t * (t * (6f * t - 15f) + 10f);

        if (t <= lowerTreshold)
        {
            if (t >= beatOffset)
            {
                float a = (1f - scaleAmount) / lowerTreshold;
                float b = scaleAmount + (-a * beatOffset);
                transform.localScale = Vector3.one * (t * a + b);
            }
            else
            {
                float a = -(1f - scaleAmount) / lowerTreshold;
                float b = scaleAmount + (-a * beatOffset);
                transform.localScale = Vector3.one * (t * a + b);
            }

            t = 0;
        }
        else if (t <= higherTreshold)
        {
            t = (t - lowerTreshold) * (1 - lowerTreshold) / (higherTreshold - lowerTreshold);
            transform.localScale = Vector3.one;
        }
        else
        {
            float a = (scaleAmount - 1f) / (1f - higherTreshold);
            float b = scaleAmount - a + (-a * beatOffset);
            transform.localScale = Vector3.one * (t * a + b);
        }
    }
}
