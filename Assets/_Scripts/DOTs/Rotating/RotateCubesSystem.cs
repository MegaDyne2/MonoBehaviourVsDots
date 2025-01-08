using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct RotateCubesSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnCubesConfig>();
    }
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        var spawnConfig = SystemAPI.GetSingleton<SpawnCubesConfig>();
        
        if (spawnConfig.useMultiThreading)
        {
            // Multi-threaded job execution
            var job = new HandleCubesJob
            {
                DeltaTime = deltaTime
            };
            state.Dependency = job.ScheduleParallel(state.Dependency);
        }
        else
        {
            // Single-threaded execution
            foreach (RotatingMovingCubeAspect rotatingMovingCubeAspect in
                     SystemAPI.Query<RotatingMovingCubeAspect>())
            {
                rotatingMovingCubeAspect.DoRotate(deltaTime);
            }
        }
    }
}

[BurstCompile]
public partial struct HandleCubesJob : IJobEntity
{
    public float DeltaTime;
    public int IterationCount;
    public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
    {
        quaternion rotation = localTransform.Rotation;
        quaternion newQuaternion = Global.CalculateNewRotation(rotateSpeed.value, rotation, DeltaTime, IterationCount);
        localTransform.Rotation = newQuaternion;

    }
}