using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;


public class ChunkShaderController : UnitySingleton<ChunkShaderController>
{
    // Reference to World.
    //World world;
    // Shader and kernels.
    public ComputeShader chunkShader;
    int PopulateVoxelMap;
    int AddVoxelDataToChunk;
    int ClearUnusedData;
    // Dimensions of the chunk.
    public Vector3 dimensions;
    public static int volume;
    //readonly int maxTriangleCount = dimensionsVolume * 6 * 2;


    // Start is called before the first frame update
    void Start()
    {
        dimensions = World.Instance.chunkDimensions;
        volume = (int)(dimensions.x * dimensions.y * dimensions.z);

        // Compute Shader kernels
        PopulateVoxelMap = chunkShader.FindKernel("PopulateVoxelMap");
        AddVoxelDataToChunk = chunkShader.FindKernel("AddVoxelDataToChunk");
        ClearUnusedData = chunkShader.FindKernel("ClearUnusedData");
        // Set chunk dimensions and triangle budget.
        chunkShader.SetInts("_dimms", (int)dimensions.x, (int)dimensions.y, (int)dimensions.z);
        chunkShader.SetInt("_triangleBudget", World.Instance.triangleBudget);
    }

    public void GenerateVoxelMap(ChunkCoordinate coord, ChunkData chunk)
    {
        chunkShader.SetInt("_positionX", (int)(coord.x * dimensions.x));
        chunkShader.SetInt("_positionY", (int)(coord.y * dimensions.y));
        chunkShader.SetInt("_positionZ", (int)(coord.z * dimensions.z));
        chunkShader.SetFloat("_frequency", World.Instance.frequency);
        chunkShader.SetFloat("_octaves", World.Instance.octaves);
        chunkShader.SetFloat("_amplitude", World.Instance.amplitude);
        chunkShader.SetFloat("_persist", World.Instance.persistance);
        chunkShader.SetFloat("_lacun", World.Instance.lacunarity);
        chunkShader.SetBuffer(PopulateVoxelMap, "_voxelMap", chunk.voxelMap);
        chunkShader.DispatchThreads(PopulateVoxelMap, World.Instance.chunkDimensions);
    }

    public void DrawMesh(ChunkCoordinate ccoord, ComputeBuffer voxelMap)
    {     
        World.Instance.vertexBuffer ??= World.Instance.mesh.GetVertexBuffer(0);
        World.Instance.normalBuffer ??= World.Instance.mesh.GetVertexBuffer(1);
        World.Instance.texcoordBuffer ??= World.Instance.mesh.GetVertexBuffer(2);
        World.Instance.indexBuffer ??= World.Instance.mesh.GetIndexBuffer();

        chunkShader.SetInt("_positionX", (int)(ccoord.x * dimensions.x));
        chunkShader.SetInt("_positionY", (int)(ccoord.y * dimensions.y));
        chunkShader.SetInt("_positionZ", (int)(ccoord.z * dimensions.z));
        chunkShader.SetBuffer(AddVoxelDataToChunk, "_vertexBuffer", World.Instance.vertexBuffer);
        chunkShader.SetBuffer(AddVoxelDataToChunk, "_normalBuffer", World.Instance.normalBuffer);
        chunkShader.SetBuffer(AddVoxelDataToChunk, "_texcoordBuffer", World.Instance.texcoordBuffer);
        chunkShader.SetBuffer(AddVoxelDataToChunk, "_indexBuffer", World.Instance.indexBuffer);
        chunkShader.SetBuffer(AddVoxelDataToChunk, "_voxelMap", voxelMap);
        chunkShader.SetBuffer(AddVoxelDataToChunk, "_count", World.Instance.countBuffer);
        chunkShader.SetInt("_triangleBudget", World.Instance.triangleBudget);
        chunkShader.DispatchThreads(AddVoxelDataToChunk, dimensions);
    }

    public void ClearRest()
    {
        chunkShader.SetBuffer(ClearUnusedData, "_vertexBuffer", World.Instance.vertexBuffer);
        chunkShader.SetBuffer(ClearUnusedData, "_normalBuffer", World.Instance.normalBuffer);
        chunkShader.SetBuffer(ClearUnusedData, "_texcoordBuffer", World.Instance.texcoordBuffer);
        chunkShader.SetBuffer(ClearUnusedData, "_indexBuffer", World.Instance.indexBuffer);
        chunkShader.SetBuffer(ClearUnusedData, "_count", World.Instance.countBuffer);
        chunkShader.DispatchThreads(ClearUnusedData, dimensions);
    }

    public void RenderChunk(ChunkData chunk, ChunkCoordinate ccoord)
    {
        DrawMesh(ccoord, chunk.voxelMap);
        //ClearRest();
    }
}

