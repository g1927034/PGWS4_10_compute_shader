using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

struct Particle
{
    public Vector3 pos;
    public Vector3 vel;
    public Vector3 col;
}

public class ParticleBehaviourScript : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] ComputeShader computeSheder;
    private int updateKernel;
    private ComputeBuffer buffer;

    static int THREAD_NUM = 64;
    static int PARTICLE_NUM = ((65536 + THREAD_NUM - 1) / THREAD_NUM) * THREAD_NUM;

    private void OnEnable()
    {
        buffer = new ComputeBuffer(
            PARTICLE_NUM,
            Marshal.SizeOf(typeof(Particle)),
            ComputeBufferType.Default);

        var initKernel = computeSheder.FindKernel("initialize");
        computeSheder.SetBuffer(initKernel, "Particles", buffer);
        computeSheder.Dispatch(initKernel, PARTICLE_NUM / THREAD_NUM, 1, 1);

        updateKernel = computeSheder.FindKernel("update");
        computeSheder.SetBuffer(updateKernel, "Particles", buffer);

        material.SetBuffer("Particles", buffer);
    }

    private void OnDisable()
    {
        buffer.Release();
    }

    // Update is called once per frame
    void Update()
    {
        computeSheder.SetFloat("deltaTime", Time.deltaTime);
        computeSheder.Dispatch(updateKernel, PARTICLE_NUM / THREAD_NUM, 1, 1);
    }
    private void OnRenderObject()
    {
        material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points, PARTICLE_NUM);
    }
}
