using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChangeOnRhythm : MonoBehaviour
{
    private float currentTime;
    private int numOfBeats = 0;

    public float lBound = 1f;
    public float uBound = 4f;

    void Update()
    {
        currentTime = Test_BeatManager.instance.songPosition - numOfBeats * Test_BeatManager.instance.secPerBeatAfterPitch;
        if (currentTime / Test_BeatManager.instance.secPerBeatAfterPitch >= 1)
        {
            Color color = new Color(Random.Range(lBound, uBound), Random.Range(lBound, uBound), Random.Range(lBound, uBound));
            gameObject.GetComponent<MeshRenderer>().material.color = color;

            numOfBeats += 1;
        }
    }
}
