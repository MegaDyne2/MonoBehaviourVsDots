using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    [SerializeField] private UIController uIController;
    [SerializeField] private GameObject prefabBullet;
    
    public float speed = 10f; // Movement speed
    public float lookSpeed = 2f; // Look sensitivity

    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        // Mouse look
        yaw += Input.GetAxis("Mouse X") * lookSpeed;
        pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // WASD movement
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetKey(KeyCode.E) ? 1 : Input.GetKey(KeyCode.Q) ? -1 : 0,
            Input.GetAxis("Vertical")
        );
        transform.position += transform.TransformDirection(move) * speed * Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uIController.SetFlyCameraActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject go = Instantiate(prefabBullet, transform.position, Quaternion.identity);
            MonoBehaviourBullet bullet = go.GetComponent<MonoBehaviourBullet>();
            Rigidbody rb = go.GetComponent<Rigidbody>();
            
            rb.linearVelocity = transform.TransformDirection(Vector3.forward) * bullet.speed;
            
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            
        }
    }
}