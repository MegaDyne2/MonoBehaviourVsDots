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
    public float bulletSpeed = 100f;
    
    #endregion

    #region Private Variable

    private float _yaw = 0f;
    private float _pitch = 0f;
    private SpawnEntitiesSystem _spawnerDots;
    #endregion

    #region Unity Functions

    void Update()
    {
        HandleMouseLook();
        HandleMouseLookAndKeyboardMovement();
        HandleMouseFireButton();
    }

    #endregion

    #region Private Functions

    private void HandleMouseLook()
    {
        // Mouse look
        _yaw += Input.GetAxis("Mouse X") * lookSpeed;
        _pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    private void HandleMouseLookAndKeyboardMovement()
    {
        // WASD movement
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetKey(KeyCode.E) ? 1 : Input.GetKey(KeyCode.Q) ? -1 : 0,
            Input.GetAxis("Vertical")
        );

        transform.position += transform.TransformDirection(move) * (speed * Time.deltaTime);
    }

    private void HandleMouseFireButton()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Global.IsDots)
            {
                if (_spawnerDots == null)
                    _spawnerDots = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpawnEntitiesSystem>();

                _spawnerDots.SpawnBullet(transform.position, transform.TransformDirection(Vector3.forward) * bulletSpeed);
                
            }
            else
            {
                GameObject go = Instantiate(prefabBullet, transform.position, Quaternion.identity);
                MonoBehaviourBullet bullet = go.GetComponent<MonoBehaviourBullet>();
                Rigidbody rb = go.GetComponent<Rigidbody>();

                rb.linearVelocity = transform.TransformDirection(Vector3.forward) * bulletSpeed;
            }
        }
    }



    #endregion
}