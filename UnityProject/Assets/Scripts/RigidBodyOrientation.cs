using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Example of running a simple compute shader to fill a buffer.
/// </summary>
public class RigidBodyOrientation : MonoBehaviour
{
    public GameObject cube;
    public Vector3 force = new Vector3(0, 0, 1);
    public Vector3 pointOfForce = new Vector3(1, 0, -1);

    struct Particle
    {
        public Vector3 force;
        public Vector3 pf;
        public Vector3 torque;
        public Vector4 rotation;
    }
    private int particleStructSize = 13;

    public ComputeShader shader;
    private ComputeBuffer buffer;

    private int kernelHandle;
    private int groupCount = 1;
    private int threadCount = 4;

    private int bufferSize;
    private Particle[] particles;

    private int cubeCount;

    private void Start()
    {
        cubeCount = threadCount * groupCount;

        // Calculate the buffer size.
        bufferSize = cubeCount;
        particles = new Particle[bufferSize];

        // Create compute buffer.
        buffer = new ComputeBuffer(bufferSize, sizeof(float) * particleStructSize);

        // Obtain the handle to the kernel to run.
        kernelHandle = shader.FindKernel("CSMain");

        // Generate the particles, using the positions of the ball game objects.
        Particle[] initialBufferData = new Particle[cubeCount];
        for (int i = 0; i < cubeCount; ++i)
        {
            Particle particle = new Particle();
            particle.force = force;
            particle.pf = pointOfForce;
            particle.rotation = new Vector4(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
            initialBufferData[i] = particle;
        }

        // Set the data.
        buffer.SetData(initialBufferData);

        // Set the buffer on the compute shader.
        shader.SetBuffer(kernelHandle, "buffer", buffer);
    }

    private void Update()
    {
        shader.SetFloat("duration", Time.deltaTime);

        shader.Dispatch(kernelHandle, groupCount, 1, 1);

        buffer.GetData(particles);

        cube.transform.RotateAround(Vector3.zero, particles[0].torque, 30 * Time.deltaTime);
    }

    private void OnDestroy()
    {
        // Release the buffer.
        buffer.Release();
    }
}