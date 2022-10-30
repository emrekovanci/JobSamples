using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

using UnityEngine;

namespace Demos.Wave
{
    [BurstCompile]
    public struct UpdateWaveJob : IJobParallelFor
    {
        public NativeArray<Vector3> Vertices;
        public NativeArray<Vector3> Normals;

        [ReadOnly]
        public float Scale;

        [ReadOnly]
        public float Height;

        [ReadOnly]
        public float OffsetSpeed;

        [ReadOnly]
        public float Time;

        public void Execute(int i)
        {
            if (Normals[i].z > 0f)
            {
                Vector3 vertex = Vertices[i];

                float noiseValue = Noise(vertex.x * Scale + OffsetSpeed * Time, vertex.y * Scale + OffsetSpeed * Time);

                Vertices[i] = new Vector3(vertex.x, vertex.y, noiseValue * Height * 0.3f);
            }
        }

        private static float Noise(float x, float y)
        {
            float2 pos = math.float2(x, y);
            return noise.snoise(pos);
        }
    }
}