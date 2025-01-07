using Unity.Entities;
using Unity.Transforms;

public readonly partial struct MovingAspect : IAspect
{
    public readonly RefRW<LocalTransform> localTransform;
    public readonly RefRO<MovingComponent> direction;

    public void Move(float deltaTime)
    {
        localTransform.ValueRW = localTransform.ValueRO.Translate(direction.ValueRO.velocity * deltaTime);
    }

}
