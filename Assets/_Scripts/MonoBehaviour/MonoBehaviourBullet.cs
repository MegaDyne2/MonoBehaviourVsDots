using System;
using UnityEngine;

public class MonoBehaviourBullet : MonoBehaviour
{
    public float lifetime = 5f;
    public float speed = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        MonoBehaviourPrefabController monoBehaviourPrefabController = other.GetComponent<MonoBehaviourPrefabController>();

        if (monoBehaviourPrefabController == null)
            return;

        Destroy(monoBehaviourPrefabController.gameObject);
        //monoBehaviourPrefabController.speed *= -2;
        
        
    }

    private void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

            
        //Debug.Log( rb.linearVelocity);
    }
}
