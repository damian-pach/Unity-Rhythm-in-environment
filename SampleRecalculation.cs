using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class SampleRecalculation : MonoBehaviour
{

    private AudioSource _audioSource;
    public static float[] _samples = new float[512];
    public float[] _freqBand = new float[8];
    public float[] _bandBuffer = new float[8];
    private float[] _bufferDecrease = new float[8];

    private float[] _freqBandHighest = new float[8];
    public float[] _audioBand = new float[8];
    public float[] _audioBandBuffer = new float[8];

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
    }

    private void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    private void MakeFrequencyBands()
    {
        int count = 0;
       
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count];
                count++;
            }

            _freqBand[i] = average;
        }
    }

    private void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            if(_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }

            if(_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if(_freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = _freqBand[i] / _freqBandHighest[i];
            _audioBandBuffer[i] = _bandBuffer[i] / _freqBandHighest[i];
        }
    }


}
