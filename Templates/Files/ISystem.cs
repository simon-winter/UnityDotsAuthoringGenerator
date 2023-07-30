using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public partial struct TEMPLATENAME_ : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {

        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().
            CreateCommandBuffer(state.WorldUnmanaged);

        state.Dependency = new TEMPLATENAME_Job {
            Ecb = ecb.AsParallelWriter(),
        }.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
public partial struct TEMPLATENAME_Job : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Ecb;

    [BurstCompile]
    public void Execute(Entity entity) {

    }
}