using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5f;
    public float maxBounceAngle = 45f;
    public Mesh[] meshes; // Array of Mesh objects to change the mesh of the ball

    private MeshFilter meshFilter;
    public Vector2 direction;
    private Rigidbody2D rb;

    private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        meshFilter = GetComponent<MeshFilter>(); // Get the MeshFilter component
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        mainCamera = Camera.main;
        cameraHeight = 2f * mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;

        Vector2 position = transform.position;
        if (position.x < -cameraWidth / 2f && direction.x < 0f)
        {
            direction.x = -direction.x;
        }
        if (position.x > cameraWidth / 2f && direction.x > 0f)
        {
            direction.x = -direction.x;
        }
        if (position.y < -cameraHeight / 2f && direction.y < 0f)
        {
            direction.y = -direction.y;
        }
        if (position.y > cameraHeight / 2f && direction.y > 0f)
        {
            direction.y = -direction.y;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            float hitPoint = collision.contacts[0].point.y - collision.gameObject.transform.position.y;
            float normalizedHitPoint = hitPoint / (collision.collider.bounds.size.y / 2);
            float angle = normalizedHitPoint * maxBounceAngle;

            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }

    // Change the mesh of the ball when the volume is above the threshold
    public void ChangeMesh()
    {
        if (meshes.Length > 0)
        {
            Mesh newMesh = meshes[Random.Range(0, meshes.Length)];
            meshFilter.mesh = newMesh;
        }
    }
}
