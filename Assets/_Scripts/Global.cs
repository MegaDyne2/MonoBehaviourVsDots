using Unity.Mathematics;
using UnityEngine;

public static class Global
{
    public static bool IsMultiThreaded = false;
    public static int IterationCount = 100; //I cannot use this inside CalculateNewRotation by itself due to [BurstCompile]
    public static bool IsDots = false;
    
    public static Quaternion CalculateNewRotation(float newSpeed, Quaternion originalQuaterion, float deltaTime, int iterationCount)
    {
        for (int i = 0; i < iterationCount; i++)
        {
            newSpeed *= 1.0001f;
            if (newSpeed >= 360f)
            {
                newSpeed -= 360f;
            }
        }

        quaternion rotation = math.mul(
            originalQuaterion,
            quaternion.RotateY(math.radians(newSpeed * deltaTime))
        );
        return rotation;
    }
}
