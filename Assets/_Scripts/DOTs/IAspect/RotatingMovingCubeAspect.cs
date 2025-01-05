using Unity.Burst;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public readonly partial struct RotatingMovingCubeAspect : IAspect
{
    // public readonly RefRO<RotatingCube> rotatingCube;
    public readonly RefRW<LocalTransform> localTransform;
    public readonly RefRO<RotateSpeed> rotateSpeed;
    //public readonly RefRO<Movement> movement;


    public void MoveAndRotate(float deltaTime)
    {
        //Debug.unityLogger.Log($"Dots: {rotateSpeed.ValueRO.value} | {deltaTime} | {rotateSpeed.ValueRO.value * deltaTime}");
        localTransform.ValueRW = localTransform.ValueRO.RotateY(math.radians(rotateSpeed.ValueRO.value * deltaTime));

    }
}
