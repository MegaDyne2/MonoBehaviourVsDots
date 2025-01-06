using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct HandleCubesSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        // Ensure the system only runs if SpawnCubesConfig exists
        state.RequireForUpdate<SpawnCubesConfig>();
    }
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        // Get the threading mode from the SpawnCubesConfig
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
                     SystemAPI.Query<RotatingMovingCubeAspect>().WithAll<RotatingCube>())
            {
                rotatingMovingCubeAspect.MoveAndRotate(deltaTime);
            }
        }
    }
}


[BurstCompile]
public partial struct HandleCubesJob : IJobEntity
{
    public float DeltaTime;

    public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
    {
        localTransform = localTransform.RotateY(math.radians(rotateSpeed.value * DeltaTime));
    }
}