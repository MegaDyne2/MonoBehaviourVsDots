using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// For the Cube to rotate.
/// </summary>
public class MonoBehaviourPrefabCubeController : MonoBehaviour
{
    [FormerlySerializedAs("speed")] public float rotationSpeed = 5f; 
}