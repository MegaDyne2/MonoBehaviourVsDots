using Unity.Entities;
using UnityEngine;

public class MovingAuthoring : MonoBehaviour
{
    public Vector3 direction;

    private class Baker : Baker<MovingAuthoring>
    {
        public override void Bake(MovingAuthoring authoring)
        {
            Entity movingAuthoring = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(movingAuthoring, new MovingComponent { direction = authoring.direction });
        }
    }
}

public struct MovingComponent : IComponentData
{
    public Vector3 direction;
}