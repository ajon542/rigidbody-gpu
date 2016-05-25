using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Example of running a simple compute shader to fill a buffer.
/// </summary>
public class IntegrateParticle : MonoBehaviour
{
    struct Particle
    {
        public float mass;
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 acceleration;
        public float damping;
    }

    public ComputeShader shader;
    private ComputeBuffer buffer;

    private int kernelHandle;
    private int groupCount = 10;
    private int threadCount = 1024;

    private int bufferSize;
    private Particle[] particles;

    private int ballCount;
    private List<GameObject> balls;

    private void Start()
    {
        ballCount = 1024 * groupCount;

        // Calculate the buffer size.
        bufferSize = groupCount * threadCount;
        particles = new Particle[bufferSize];

        // Create compute buffer.
        buffer = new ComputeBuffer(bufferSize * 11, sizeof(float));

        // Obtain the handle to the kernel to run.
        kernelHandle = shader.FindKernel("CSMain");

        // Generate the specified number of balls game objects.
        BallGenerator bg = new BallGenerator();
        balls = bg.Generate(gameObject, ballCount, "Custom");

        // Generate the particles, using the positions of the ball game objects.
        Particle[] initialBufferData = new Particle[ballCount];
        for(int i = 0; i < ballCount; ++i)
        {
            Particle particle = new Particle();
            particle.mass = 2;
            particle.position = balls[i].transform.position;
            particle.velocity = new Vector3(0, 10, 0);
            particle.acceleration = new Vector3(0, -10f, 0);
            particle.damping = 0;
            initialBufferData[i] = particle;
        }

        // Set the data.
        buffer.SetData(initialBufferData);

        // Set the buffer on the compute shader.
        shader.SetBuffer(kernelHandle, "buffer", buffer);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
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
                //Debug.Log(particles[i].position);
                balls[i].transform.position = particles[i].position;
            }
        }
    }

    private void OnDestroy()
    {
        // Release the buffer.
        buffer.Release();
    }
}