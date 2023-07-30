using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateBefore(typeof(PhysicsSimulationGroup))]
public partial struct TEMPLATENAME_ : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {

        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var healthLookup = SystemAPI.GetComponentLookup<Health>(false);
        var damageDealerLookup = SystemAPI.GetComponentLookup<DamageDealer>();

        var triggerJob = new TEMPLATENAME_Job {
            Ecb = ecb,
            HealthLookup = healthLookup,
            DamageDealerLookup = damageDealerLookup,
        }.Schedule(simulation, state.Dependency);
        state.Dependency = triggerJob;
        triggerJob.Complete();
    }
}

[BurstCompile]
struct TEMPLATENAME_Job : ITriggerEventsJob
{
    public EntityCommandBuffer Ecb;

    public ComponentLookup<Health> HealthLookup;
    [ReadOnly] public ComponentLookup<DamageDealer> DamageDealerLookup;

    [BurstCompile]
    public void Execute(TriggerEvent triggerEvent) {
        var a = triggerEvent.EntityA;
        var b = triggerEvent.EntityB;

        if (DamageDealerLookup.TryGetComponent(a, out var damageDealer) && HealthLookup.HasComponent(b)) {
            var health = HealthLookup[b];
            health.Value -= damageDealer.Damage;
            HealthLookup[b] = health;
        }
    }
}




