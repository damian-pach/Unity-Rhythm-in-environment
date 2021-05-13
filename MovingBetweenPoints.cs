using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBetweenPoints : MonoBehaviour
{
    public Transform[] currentPoint;

    [SerializeField]
    private int targetIndex = 0;
    [SerializeField]
    private float beat;

    private float speed;
    private float timePerBeat;
    
    private float currentTime;
    public int initialPointIndex;
    private int numOfBeats = 0;


    private void Start()
    {
        beat = Test_BeatManager.instance.noteNumber;
        //Test_BeatManager.instance.ShouldPlay = true;
        timePerBeat = Test_BeatManager.instance.secPerBeatAfterPitch;
        transform.position = currentPoint[targetIndex - 1 < 0 ? currentPoint.Length - 1 : targetIndex - 1].position;
        speed = (currentPoint[targetIndex].position - currentPoint[targetIndex - 1 < 0 ? currentPoint.Length - 1 : targetIndex - 1].position).magnitude / timePerBeat;
        targetIndex = initialPointIndex;
    }

    void FixedUpdate()
    {
        if (Test_BeatManager.instance.isPlaying)
        {
            beat = Test_BeatManager.instance.noteNumber;

            currentTime = Test_BeatManager.instance.songPosition - numOfBeats * Test_BeatManager.instance.secPerBeatAfterPitch;
            if (currentTime / Test_BeatManager.instance.secPerBeatAfterPitch >= 1)
            {
                if (targetIndex + 1 < currentPoint.Length)
                {
                    targetIndex += 1;
                }
                else
                {
                    targetIndex = 0;
                }
                numOfBeats += 1;
            }

            currentTime %= Test_BeatManager.instance.secPerBeatAfterPitch;
            float t = currentTime / Test_BeatManager.instance.secPerBeatAfterPitch;
            t = t * t * t * (t * (6f * t - 15f) + 10f);

            transform.position = Vector3.Lerp(currentPoint[targetIndex - 1 < 0 ? currentPoint.Length - 1 : targetIndex - 1].position, currentPoint[targetIndex].position, t);
            transform.rotation = Quaternion.Lerp(currentPoint[targetIndex - 1 < 0 ? currentPoint.Length - 1 : targetIndex - 1].rotation, currentPoint[targetIndex].rotation, t);

        }
    }


}
