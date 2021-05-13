using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_RhythmBoxMoving : MonoBehaviour, IInteractable
{

    public GameObject BoxPrefab;

    public GameObject[] PointsForPositions;

    public int[] InitialObjectsPositions_Int;

    public bool ShouldSetEverySecondPosition = false;

    private Vector3[] pointsPosition;
    private Transform[] pointsTransform;
    private GameObject[] boxes;
    private int[] pointNumbers;
    private bool canInteract = false;
    private float secPerBeat;
    private float maxPlayingTime;
    private bool isInteracting = false;
    
    private void Start()
    {
        pointsTransform = new Transform[PointsForPositions.Length];
        boxes = new GameObject[PointsForPositions.Length];
        pointsPosition = new Vector3[PointsForPositions.Length];

        pointNumbers = new int[PointsForPositions.Length];

        for (int i = 0; i < pointNumbers.Length; i++)
        {
            pointNumbers[i] = i;
        }
        int j = 0;
        foreach (GameObject obj in PointsForPositions)
        {
            pointsTransform[j] = PointsForPositions[j].transform;
            j++;
        }
    }

    void Update()
    {
        if (!Test_BeatManager.instance.isPlaying)
        {
            canInteract = false;
            isInteracting = false;
        }
    }

    public void Interaction()
    {
        canInteract = true;
        GetDataFromSongManager();
        if (ShouldSetEverySecondPosition)
        {
            SetEverySecondBlock();
        }
        SetData();
    }

    void GetDataFromSongManager()
    {
        secPerBeat = Test_BeatManager.instance.secPerBeatAfterPitch;
        maxPlayingTime = Test_BeatManager.instance.beatsPerLoop * secPerBeat * Test_BeatManager.instance.numberOfAllowedLoops;
    }

    void SetData()
    {
        if (canInteract)
        {
            if (!isInteracting)
            {
                isInteracting = true;
                for (int i = 0; i < InitialObjectsPositions_Int.Length; i++)
                {
                    GameObject box = Instantiate(BoxPrefab);
                    box.transform.position = PointsForPositions[InitialObjectsPositions_Int[i]].transform.position;
                    box.GetComponent<MovingBetweenPoints>().currentPoint = pointsTransform;
                    box.GetComponent<MovingBetweenPoints>().initialPointIndex = InitialObjectsPositions_Int[i];
                }

                Test_BeatManager.instance.ShouldPlay = true;

            }
        }
    }

    private void SetEverySecondBlock()
    {
        InitialObjectsPositions_Int = new int[PointsForPositions.Length / 2 + 1];
        int k = 0;
        for (int i = 0; i < PointsForPositions.Length; i++)
        {
            if(i%2 == 0)
            {
                InitialObjectsPositions_Int[k] = i;
                k++;
            }
        }
    }
}
