using Unity.Entities;

public partial struct MovingSystem : ISystem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnCreate(ref SystemState state)
    {
        // Ensure the system only runs if SpawnCubesConfig exists
        state.RequireForUpdate<MovingComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
    
        foreach (MovingAspect rotatingMovingCubeAspect in
                 SystemAPI.Query<MovingAspect>().WithAll<MovingComponent>())
        {
            rotatingMovingCubeAspect.Move(deltaTime);
        }
    }
}
