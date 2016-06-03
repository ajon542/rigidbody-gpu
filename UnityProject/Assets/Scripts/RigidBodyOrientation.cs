using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Example of running a simple compute shader to fill a buffer.
/// </summary>
public class RigidBodyOrientation : MonoBehaviour
{
    struct Particle
    {
        public Vector3 force;
        public Vector3 pf;
        public Vector3 torque;
    }
    private int particleStructSize = 9;

    public ComputeShader shader;
    private ComputeBuffer buffer;

    private int kernelHandle;
    private int groupCount = 1;
    private int threadCount = 4;

    private int bufferSize;
    private Particle[] particles;

    public GameObject cube;
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
            particle.force = new Vector3(0, 0, 1);
            particle.pf = new Vector3(-1, 0, -1);
            initialBufferData[i] = particle;
        }

        // Set the data.
        buffer.SetData(initialBufferData);

        // Set the buffer on the compute shader.
        shader.SetBuffer(kernelHandle, "buffer", buffer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Fill the buffer using the compute shader.
            shader.SetFloat("duration", Time.deltaTime);

            shader.Dispatch(kernelHandle, groupCount, 1, 1);

            // Obtain the data.
            // NOTE: This is inefficient as it is essentially copying the data from the GPU.
            // Depending on what is being done, copying the data to system memory does not
            // even need to occur. For example, if we wanted to use the buffer in a shader,
            // we could simply do "material.SetBuffer("buffer", buffer);". Alternatively, we
            // could pass the buffer along to another compute shader.
            buffer.GetData(particles);

            // Display the data.
            for (int i = 0; i < bufferSize; i++)
            {
                Debug.Log(particles[i].torque);
            }
        }
    }

    private void OnDestroy()
    {
        // Release the buffer.
        buffer.Release();
    }
}