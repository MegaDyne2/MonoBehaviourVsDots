using UnityEngine;

/// <summary>
/// The bullet for the MonoBehaviour
/// </summary>
public class MonoBehaviourBullet : MonoBehaviour
{
    #region Inspector Fields

    public float lifetime = 5f;
    public float speed = 5f;

    #endregion

    #region Unity Functions

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        MonoBehaviourPrefabCubeController monoBehaviourPrefabCubeController =
            other.GetComponent<MonoBehaviourPrefabCubeController>();

        if (monoBehaviourPrefabCubeController == null)
            return;

        Destroy(monoBehaviourPrefabCubeController.gameObject);
    }

    #endregion
}