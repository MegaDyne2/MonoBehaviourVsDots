using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

/// <summary>
/// This handles the User's controls
/// </summary>
public class FlyCamera : MonoBehaviour
{
    #region Links

    [SerializeField] private UIController uIController;
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private SubScene subSceneDots;

    #endregion

    #region Inspector Fields

    public float speed = 10f; // Movement speed
    public float lookSpeed = 2f; // Look sensitivity

    #endregion

    #region Private Variable

    private float yaw = 0f;
    private float pitch = 0f;
    private SpawnCubesSystem _spawnerDOTS;
    #endregion

    #region Unity Functions

    void Update()
    {
        HandleMouseLook();
        HandleMouseLookAndKeyboardMovement();
        HandleEscapeToTurnOffFlyCamera();
        HandleMouseFireButton();
    }

    #endregion

    #region Private Functions

    private void HandleMouseLook()
    {
        // Mouse look
        yaw += Input.GetAxis("Mouse X") * lookSpeed;
        pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void HandleMouseLookAndKeyboardMovement()
    {
        // WASD movement
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetKey(KeyCode.E) ? 1 : Input.GetKey(KeyCode.Q) ? -1 : 0,
            Input.GetAxis("Vertical")
        );

        transform.position += transform.TransformDirection(move) * speed * Time.deltaTime;
    }

    private void HandleMouseFireButton()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (uIController.IsDots())
            {
                if (subSceneDots.IsLoaded == false)
                {
                    return;
                }

                if (_spawnerDOTS == null)
                    _spawnerDOTS = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpawnCubesSystem>();

                _spawnerDOTS.SpawnBullet(transform.position, transform.TransformDirection(Vector3.forward) * 100.0f);
                
            }
            else
            {
                GameObject go = Instantiate(prefabBullet, transform.position, Quaternion.identity);
                MonoBehaviourBullet bullet = go.GetComponent<MonoBehaviourBullet>();
                Rigidbody rb = go.GetComponent<Rigidbody>();

                rb.linearVelocity = transform.TransformDirection(Vector3.forward) * bullet.speed;
            }
        }
    }

    private void HandleEscapeToTurnOffFlyCamera()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uIController.SetFlyCameraActive(false);
        }
    }

    #endregion
}