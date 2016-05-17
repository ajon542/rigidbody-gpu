using UnityEngine;

/// <summary>
/// Example of running a simple compute shader to fill a buffer.
/// </summary>
public class IntegrateParticle : MonoBehaviour
{
    struct Particle
    {
        public Vector3 position;
    }

    public ComputeShader shader;
    private ComputeBuffer buffer;

    private int kernelHandle;
    private int groupCount = 1;
    private int threadCount = 4;

    private int bufferSize;
    private Particle[] particles;

    private void Start()
    {
        // Calculate the buffer size.
        bufferSize = groupCount * threadCount;
        particles = new Particle[bufferSize];

        // Create compute buffer.
        buffer = new ComputeBuffer(bufferSize * 3, sizeof(int));

        // Obtain the handle to the kernel to run.
        kernelHandle = shader.FindKernel("CSMain");

        // Create some initial data.
        Particle[] initialBufferData = new Particle[]
        {
            new Particle { position = new Vector3(1, 1, 1) },
            new Particle { position = new Vector3(2, 2, 2) },
            new Particle { position = new Vector3(3, 3, 3) },
            new Particle { position = new Vector3(4, 4, 4) },
        };
        buffer.SetData(initialBufferData);

        // Set the buffer on the compute shader.
        shader.SetBuffer(kernelHandle, "buffer", buffer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Fill the buffer using the compute shader.
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
                Debug.Log(particles[i].position);
            }
        }
    }

    private void OnDestroy()
    {
        // Release the buffer.
        buffer.Release();
    }
}