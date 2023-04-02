using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    public float sensitivity = 100f;
    public float threshold = 0.65f;

    private AudioSource audioSource;
    private float maxVolume;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 1, AudioSettings.outputSampleRate);
        audioSource.loop = true;
        audioSource.mute = true;

        while (!(Microphone.GetPosition(null) > 0)) { }

        audioSource.Play();

        maxVolume = 0f;
    }

    void Update()
    {
        float[] samples = new float[audioSource.clip.samples];
        audioSource.clip.GetData(samples, 0);

        float rms = 0f;

        foreach (float sample in samples)
        {
            rms += sample * sample;
        }

        rms /= samples.Length;
        rms = Mathf.Sqrt(rms);

        float volume = Mathf.Clamp01(rms * sensitivity);

        if (volume > maxVolume)
        {
            maxVolume = volume;
        }

        if (volume > threshold)
        {
            // Get a reference to all the Ball objects in the scene
            Ball[] balls = FindObjectsOfType<Ball>();

            // Change the direction and color of each ball
            foreach (Ball ball in balls)
            {
                ball.direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                ball.ChangeMesh();
                ball.GetComponent<Renderer>().material.color = Random.ColorHSV();
            }
        }
    }

    void OnDisable()
    {
        audioSource.Stop();
        Microphone.End(null);
    }
}
