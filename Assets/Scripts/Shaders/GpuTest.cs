using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpuTest : MonoBehaviour
{
    Vector2Int dimm;
    uint[] test2d;
    ComputeBuffer test2dBuffer;
    ComputeShader testComputeShader;

    void allocateBuffers()
    {
        dimm = new Vector2Int(8,8);
        uint[] test2d = new uint[64];
        test2dBuffer = new ComputeBuffer(64, 12);
    }

    void runShader()
    {
        testComputeShader.FindKernel("testKernel");
    }

    // Start is called before the first frame update
    void Start()
    {
        allocateBuffers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
