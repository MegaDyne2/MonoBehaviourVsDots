using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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

        if (!Global.IsDots)
        {
            return;
        }

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        if (Global.IsMultiThreaded)
        {
            // Multi-threaded job execution
            var job = new HandleCubesJob
            {
                DeltaTime = deltaTime,
                IterationCount = Global.IterationCount
            };

            // Schedule the job
            var jobHandle = job.ScheduleParallel(state.Dependency);

            // Force the job to complete
            jobHandle.Complete();

            // Record timing after the job completes
            stopwatch.Stop();
            Global.ElapsedRotationMS = stopwatch.Elapsed.TotalMilliseconds;

            // Assign job handle
            state.Dependency = jobHandle;
        }
        else
        {
            // Single-threaded execution
            foreach (RotatingMovingCubeAspect rotatingMovingCubeAspect in
                     SystemAPI.Query<RotatingMovingCubeAspect>())
            {
                rotatingMovingCubeAspect.DoRotate(deltaTime);
            }

            // Record timing for single-threaded execution
            stopwatch.Stop();
            Global.ElapsedRotationMS = stopwatch.Elapsed.TotalMilliseconds;
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
