using UnityEngine;
using System.Collections.Generic;

public class Matrix3x3
{
    public float[,] m = new float[3, 3];

    public void Print()
    {
        for (int row = 0; row < 3; ++row)
        {
            Debug.Log(m[row, 0] + " " + m[row, 1] + " " + m[row, 2]);
        }
    }

    public float[,] Invert()
    {
        // https://en.wikipedia.org/wiki/Invertible_matrix#Inversion_of_3.C3.973_matrices
        float a =  (m[1, 1] * m[2, 2] - m[1, 2] * m[2, 1]);
        float b = -(m[1, 0] * m[2, 2] - m[1, 2] * m[2, 0]);
        float c =  (m[1, 0] * m[2, 1] - m[1, 1] * m[2, 0]);
        float d = -(m[0, 1] * m[2, 2] - m[0, 2] * m[2, 1]);
        float e =  (m[0, 0] * m[2, 2] - m[0, 2] * m[2, 0]);
        float f = -(m[0, 0] * m[2, 1] - m[0, 1] * m[2, 0]);
        float g =  (m[0, 1] * m[1, 2] - m[0, 2] * m[1, 1]);
        float h = -(m[0, 0] * m[1, 2] - m[0, 2] * m[1, 0]);
        float i =  (m[0, 0] * m[1, 1] - m[0, 1] * m[1, 0]);

        float det = 1 / (m[0, 0] * a + b * m[0, 1] + c * m[0, 2]);

        float[,] im = new float[3, 3];
        im[0, 0] = a * det;
        im[0, 1] = d * det;
        im[0, 2] = g * det;
        im[1, 0] = b * det;
        im[1, 1] = e * det;
        im[1, 2] = h * det;
        im[2, 0] = c * det;
        im[2, 1] = f * det;
        im[2, 2] = i * det;

        return im;
    }
}

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


        Matrix3x3 m = new Matrix3x3();
        m.m[0, 0] = 1;
        m.m[0, 1] = 2;
        m.m[0, 2] = 3;
        m.m[1, 0] = 3;
        m.m[1, 1] = 1;
        m.m[1, 2] = -7;
        m.m[2, 0] = 2;
        m.m[2, 1] = 8;
        m.m[2, 2] = 3;
        m.Print();
        Matrix3x3 inverse = new Matrix3x3();
        inverse.m = m.Invert();
        inverse.Print();
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