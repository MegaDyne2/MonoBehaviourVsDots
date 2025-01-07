using UnityEngine;
using Unity.Entities;
using UnityEngine.Serialization;

/// <summary>
/// From "Freedom Coding"
/// </summary>
public class TriggerAuthoring : MonoBehaviour
{
    [FormerlySerializedAs("size")] public float radianSize;

    public class Baker : Baker<TriggerAuthoring>
    {
        public override void Bake(TriggerAuthoring authoring)
        {
            Entity triggerAuthoring = GetEntity(TransformUsageFlags.None);

            AddComponent(triggerAuthoring, new SphereTriggerComponent { RadianSize = authoring.radianSize });
        }
    }
}
