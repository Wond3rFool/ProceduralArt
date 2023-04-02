using UnityEngine;

public class BouncingBall : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnInterval = 2f;
    public float ballSpeed = 5f;
    public float maxBounceAngle = 45f;

    public MicrophoneInput micInput;

    private Camera mainCamera;
    private float cameraWidth;
    private float cameraHeight;

    private int ballsSpawned = 0;

    void Start()
    {
        mainCamera = Camera.main;
        cameraHeight = 2f * mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;

        InvokeRepeating("SpawnBall", 0f, spawnInterval);
    }

    void SpawnBall()
    {
        if (ballsSpawned < 10) // spawn only 10 balls
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-cameraWidth / 2f, cameraWidth / 2f), Random.Range(-cameraHeight / 2f, cameraHeight / 2f));
            GameObject ballObject = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            Ball ballScript = ballObject.GetComponent<Ball>();
            ballScript.maxBounceAngle = maxBounceAngle;

            // Set the ball color based on the microphone input
            Color baseColor = Color.Lerp(Color.white, Color.red, micInput.sensitivity);
            float hue, saturation, brightness;
            Color.RGBToHSV(baseColor, out hue, out saturation, out brightness);
            saturation = Mathf.Clamp01(saturation * 2f); // Increase saturation
            brightness = Mathf.Clamp01(brightness * 2f); // Increase brightness
            Color color = Color.HSVToRGB(hue, saturation, brightness);
            ballScript.GetComponent<Renderer>().material.color = color;

            // Apply a random force to the ball
            Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * ballSpeed;
            ballScript.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

            ballsSpawned++;
        }
        else
        {
            CancelInvoke("SpawnBall"); // stop spawning balls
        }
    }
}