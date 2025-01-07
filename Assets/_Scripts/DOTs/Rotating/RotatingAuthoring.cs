using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class RotatingAuthoring : MonoBehaviour
{
    [FormerlySerializedAs("value")] public float rotatingSpeed = 3;

    private class Baker : Baker<RotatingAuthoring>
    {
        public override void Bake(RotatingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotateSpeed
            {
                value = authoring.rotatingSpeed
            });
        }
    }
}

public struct RotateSpeed : IComponentData
{
    public float value;
    
}
