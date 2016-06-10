using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Example of running a simple compute shader to fill a buffer.
/// </summary>
public class RigidBodyOrientation : MonoBehaviour
{
    public GameObject cube;
    public float mass = 10;
    public Vector3 force = new Vector3(0, 0, 1);
    public Vector3 pointOfForce = new Vector3(1, 0, -1);

    struct RigidBody
    {
        public float mass;
        public Vector3 force;
        public Vector3 pointOfForce;
        public Vector3 torque;
        public Vector3 rotation;
        public Vector4 orientation;
    }
    private int rigidBodyStructSize = 17;

    public ComputeShader shader;
    private ComputeBuffer buffer;

    private int kernelHandle;
    private int groupCount = 1;
    private int threadCount = 4;

    private int bufferSize;
    private RigidBody[] rigidBodies;

    private int cubeCount;

    private void Start()
    {
        cubeCount = threadCount * groupCount;

        // Calculate the buffer size.
        bufferSize = cubeCount;
        rigidBodies = new RigidBody[bufferSize];

        // Create compute buffer.
        buffer = new ComputeBuffer(bufferSize, sizeof(float) * rigidBodyStructSize);

        // Obtain the handle to the kernel to run.
        kernelHandle = shader.FindKernel("CSMain");

        // Generate the particles, using the positions of the ball game objects.
        RigidBody[] initialBufferData = new RigidBody[cubeCount];
        for (int i = 0; i < cubeCount; ++i)
        {
            RigidBody rigidBody = new RigidBody();
            rigidBody.mass = mass;
            rigidBody.force = force;
            rigidBody.pointOfForce = pointOfForce;
            rigidBody.rotation = new Vector3(0, 0, 0);
            rigidBody.orientation = new Vector4(0, 0, 0, 1);
            initialBufferData[i] = rigidBody;
        }

        // Set the data.
        buffer.SetData(initialBufferData);

        // Set the buffer on the compute shader.
        shader.SetBuffer(kernelHandle, "buffer", buffer);
    }

    private void Update()
    {
        Vector3 pointOfForce = Vector3.zero;

        shader.SetFloat("duration", Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            // create a ray going into the scene from the screen location the user clicked at
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // the raycast hit info will be filled by the Physics.Raycast() call further
            RaycastHit hit;

            // perform a raycast using our new ray. 
            // If the ray collides with something solid in the scene, the "hit" structure will
            // be filled with collision information
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);

                pointOfForce = hit.point;
            }
        }

        shader.Dispatch(kernelHandle, groupCount, 1, 1);

        buffer.GetData(rigidBodies);

        cube.transform.rotation = new Quaternion
        (
            rigidBodies[0].orientation.x,
            rigidBodies[0].orientation.y,
            rigidBodies[0].orientation.z,
            rigidBodies[0].orientation.w
        );

        if (pointOfForce != Vector3.zero)
        {
            rigidBodies[0].pointOfForce = pointOfForce;
            rigidBodies[0].force = force;
        }

        buffer.SetData(rigidBodies);
    }

    private void OnDestroy()
    {
        // Release the buffer.
        buffer.Release();
    }
}