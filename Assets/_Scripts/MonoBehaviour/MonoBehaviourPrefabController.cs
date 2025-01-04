using UnityEngine;

public class MonoBehaviourPrefabController : MonoBehaviour
{
    public float speed = 5f; // Speed of the boid


    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}