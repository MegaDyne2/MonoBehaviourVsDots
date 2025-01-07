using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This is use for the Object to translate to a new location
/// </summary>
public class MovingAuthoring : MonoBehaviour
{
    //rename for clarity. the speed will also be applied here along with direction
    [FormerlySerializedAs("direction")] public Vector3 velocity;

    private class Baker : Baker<MovingAuthoring>
    {
        public override void Bake(MovingAuthoring authoring)
        {
            Entity movingAuthoring = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(movingAuthoring, new MovingComponent { velocity = authoring.velocity });
        }
    }
}

public struct MovingComponent : IComponentData
{
    public Vector3 velocity;
}