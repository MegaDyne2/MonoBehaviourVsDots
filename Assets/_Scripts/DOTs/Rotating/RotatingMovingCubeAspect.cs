using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public readonly partial struct RotatingMovingCubeAspect : IAspect
{
    public readonly RefRW<LocalTransform> localTransform;
    public readonly RefRO<RotateSpeed> rotateSpeed;
    
    public void DoRotate(float deltaTime)
    {
        localTransform.ValueRW.Rotation = Global.CalculateNewRotation(rotateSpeed.ValueRO.value, localTransform.ValueRW.Rotation, deltaTime, Global.IterationCount);
    }

}
