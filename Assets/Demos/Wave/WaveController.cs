using Unity.Collections;
using Unity.Jobs;

using UnityEngine;

namespace Demos.Wave
{
    public class WaveController : MonoBehaviour
    {
        [Header("Wave Parameters")]
        public float WaveScale;
        public float WaveHeight;
        public float WaveOffsetSpeed;

        [Header("Model")]
        public MeshFilter WaterMeshFilter;
        private Mesh _WaterMesh;

        private NativeArray<Vector3> _WaterVertices;
        private NativeArray<Vector3> _WaterNormals;

        private UpdateWaveJob _UpdateWaveJob;
        private JobHandle _UpdateWaveJobHandle;

        private void Start()
        {
            _WaterMesh = WaterMeshFilter.mesh;
            _WaterMesh.MarkDynamic();

            _WaterVertices = new NativeArray<Vector3>(_WaterMesh.vertices, Allocator.Persistent);
            _WaterNormals = new NativeArray<Vector3>(_WaterMesh.normals, Allocator.Persistent);
        }

        private void Update()
        {
            _UpdateWaveJob = new UpdateWaveJob
            {
                Vertices = _WaterVertices,
                Normals = _WaterNormals,
                OffsetSpeed = WaveOffsetSpeed,
                Time = Time.time,
                Scale = WaveScale,
                Height = WaveHeight
            };

            _UpdateWaveJobHandle = _UpdateWaveJob.Schedule(_WaterVertices.Length, 64);
        }

        private void LateUpdate()
        {
            _UpdateWaveJobHandle.Complete();

            _WaterMesh.SetVertices(_UpdateWaveJob.Vertices);
            _WaterMesh.RecalculateNormals();
        }

        private void OnDestroy()
        {
            _WaterVertices.Dispose();
            _WaterNormals.Dispose();
        }
    }
}