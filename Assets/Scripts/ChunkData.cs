using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// It is used to store the voxelMap of a chunk.
public class ChunkData  
{
    // The voxelMap of the chunk.
    public ComputeBuffer voxelMap;
    // Coordinates of the chunk.
    public ChunkCoordinate chunkCoordinate;
    // World reference.
    //World world;


    public ChunkData(ChunkCoordinate chunkCoord)
    {
        chunkCoordinate = chunkCoord;

        voxelMap = new ComputeBuffer(World.Instance.chunkVolume, 4, ComputeBufferType.Structured);
    }

    public void DisposeChunk()
    {
        voxelMap.Dispose();
    }

/*
    public void Initalize(ChunkCoordinate chunkCoord, World _world)
    {
        chunkCoordinate = chunkCoord;
        world = _world;

        voxelMap = new ComputeBuffer(World.Instance.chunkVolume, 4, ComputeBufferType.Structured);
    }
    */
}