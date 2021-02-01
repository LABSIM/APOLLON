using System.Collections;
using System.Collections.Generic;

[UnityEngine.RequireComponent(typeof(UnityEngine.AudioLowPassFilter))]
public class ApollonWhiteNoiseAudioCommand
    : UnityEngine.MonoBehaviour
{

    private float sampling_frequency = 48000;

    [UnityEngine.Range(0f, 1f)]
    public float noiseRatio = 0.5f;

    // for noise part
    [UnityEngine.Range(-1f, 1f)]
    public float offset;

    public float cutoffOn = 800;
    public float cutoffOff = 100;

    public bool cutOff;

    // for tonal part
    public float frequency = 440f;
    public float gain = 0.05f;

    private float increment;
    private float phase;

    System.Random rand = new System.Random();
    UnityEngine.AudioLowPassFilter lowPassFilter;

    void Awake()
    {
        sampling_frequency = UnityEngine.AudioSettings.outputSampleRate;

        lowPassFilter = GetComponent<UnityEngine.AudioLowPassFilter>();
        Update();
    }


    void OnAudioFilterRead(float[] data, int channels)
    {
        float tonalPart = 0;
        float noisePart = 0;

        // update increment in case frequency has changed
        increment = frequency * 2f * UnityEngine.Mathf.PI / sampling_frequency;

        for (int i = 0; i < data.Length; i++)
        {

            // noise
            noisePart = noiseRatio * (float)(rand.NextDouble() * 2.0 - 1.0 + offset);

            phase = phase + increment;
            if (phase > 2 * UnityEngine.Mathf.PI) phase = 0;

            // tone
            tonalPart = (1f - noiseRatio) * (float)(gain * UnityEngine.Mathf.Sin(phase));
            
            // together
            data[i] = noisePart + tonalPart;

            // if we have stereo, we copy the mono data to each channel
            if (channels == 2)
            {
                data[i + 1] = data[i];
                i++;
            }

        }

    }

    void Update()
    {
        lowPassFilter.cutoffFrequency = cutOff ? cutoffOn : cutoffOff;
    }

}
