using UnityEngine;

/// <summary>
/// Example of running a simple compute shader to fill a buffer.
/// </summary>
public class RunCompute : MonoBehaviour
{
    public ComputeShader shader;
    private ComputeBuffer buffer;

    private int kernelHandle;
    private int groupCount = 2;
    private int threadCount = 4;

    private int bufferSize;
    private int[] data;

    private void Start()
    {
        // Calculate the buffer size.
        bufferSize = groupCount * threadCount;
        data = new int[bufferSize];

        // Create compute buffer.
        buffer = new ComputeBuffer(bufferSize, sizeof(int));

        // Obtain the handle to the kernel to run.
        kernelHandle = shader.FindKernel("CSMain");

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
            buffer.GetData(data);

            // Display the data.
            for (int i = 0; i < bufferSize; i++)
            {
                Debug.Log(data[i]);
            }
        }
    }

    private void OnDestroy()
    {
        // Release the buffer.
        buffer.Release();
    }
}