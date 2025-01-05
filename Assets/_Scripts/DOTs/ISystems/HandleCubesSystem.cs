using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct HandleCubesSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (RotatingMovingCubeAspect rotatingMovingCubeAspect in
                 SystemAPI.
                     Query<RotatingMovingCubeAspect>().
                     WithAll<RotatingCube>())
        {
            
            rotatingMovingCubeAspect.MoveAndRotate(deltaTime);
            

        }
    }
}
