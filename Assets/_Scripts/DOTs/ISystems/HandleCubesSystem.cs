using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct HandleCubesSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // foreach ((var localTransform, var rotateSpeed, var movement) in
        //     SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>, RefRO<Movement>>().WithAll<RotatingCube>())
        foreach (RotatingMovingCubeAspect rotatingMovingCubeAspect in
                 SystemAPI.Query<RotatingMovingCubeAspect>().WithAll<RotatingCube>())
        {
            
            rotatingMovingCubeAspect.MoveAndRotate(SystemAPI.Time.DeltaTime);
            

        }
    }
}
