using Unity.Burst;
using Unity.Entities;

namespace UnityDotsAuthoringGenerator.Templates.Files
{
    partial class Snippets : ISystem
    {

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {

            // snippet Ecb start
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().
                CreateCommandBuffer(state.WorldUnmanaged);
            // snippet stop
        }

        // snippet job start
        [BurstCompile]
        public partial struct MyJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter Ecb;

            [BurstCompile]
            public void Execute(Entity entity) {

            }
        }
        // snippet stop
    }
}
