using System.Security.Cryptography;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace _Scripts
{
    public partial struct RotatingCubeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RotateSpeed>();
        }

        //Limitation : unmanage types.
        // test have problems struct is better.
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            return;
            
            // OnUpdate_Old(state);

            RotatingCubeJob rotatingCubeJob = new RotatingCubeJob
            {
                deltatTime = SystemAPI.Time.DeltaTime,
            };
            
            //run it on one thread
            //rotatingCubeJob.Schedule();

            //If you want to run code below it when it finished
            //rotatingCubeJob.Schedule(state.Dependency).Complete();
            
            //this means job must finished before it can run.
            //state.Dependency = rotatingCubeJob.Schedule(state.Dependency);

            //this still runing one thread.
            //this is because we only have a few items in game that use it.
            rotatingCubeJob.ScheduleParallel();
        }
        
        [BurstCompile]
        private void OnUpdate_Old(ref SystemState state)
        {
            //RefRW = read and write
            //RefRO = Read Only
//            foreach ((RefRW<LocalTransform> localTransform, RefRO<RotateSpeed> rotateSpeed)
            foreach (var (localTransform, rotateSpeed)
//                     in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>().WithNone<Player>())
                        in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>().WithAll<RotatingCube>())
            {

                float power = 1f;
                for (int i = 0; i < 100000; i++)
                {
                    power *= 2f;
                    power /= 2f;
                }
                
                localTransform.ValueRW = localTransform.ValueRO.RotateY(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime);
                
            }
 
        }
        
        
        
        //Yeah this makes it a heck faster.
        [BurstCompile]
        //[WithNone(typeof(Player))]
        [WithAll(typeof(RotatingCube))]
        public partial struct RotatingCubeJob : IJobEntity
        {
            public float deltatTime;
            public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
            {
                float power = 1f;
                for (int i = 0; i < 100000; i++)
                {
                    power *= 2f;
                    power /= 2f;
                }
                
                localTransform = localTransform.RotateY(rotateSpeed.value * deltatTime);

            }
        }
        
    }

    


    
}