using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The bullet for the MonoBehaviour
/// </summary>
public class MonoBehaviourBullet : MonoBehaviour
{
    #region Inspector Fields

    public float lifetime = 5f;

    #endregion

    #region Static Fields

    private static readonly List<MonoBehaviourBullet> ListBullets = new();

    #endregion

    #region Unity Functions

    private void Start()
    {
        ListBullets.Add(this);
        Destroy(gameObject, lifetime);
    }

    public void OnDestroy()
    {
        ListBullets.Remove(this);
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

    #region Static Functions

    public static void DeleteAllBullets()
    {
        foreach (var currentBullet in ListBullets)
        {
            Destroy(currentBullet.gameObject);
        }

        ListBullets.Clear();
    }

    #endregion
}