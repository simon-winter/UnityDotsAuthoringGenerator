using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[InternalBufferCapacity(50)]
public partial struct TEMPLATENAME_ : IBufferElementData
{
    public Entity Entity;
    public float a;
    public float3 b;
}