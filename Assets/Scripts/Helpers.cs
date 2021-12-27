using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class ChunkCoordinete - A chunk coordinate is a coordinate in the world that is used to identify a chunk.
public class ChunkCoordinate
{
    public int x;
    public int y;
    public int z;

    public ChunkCoordinate()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public ChunkCoordinate(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public bool Equals(ChunkCoordinate other)
    {
        if(other == null)
            return false;
        else if(other.x == x && other.y == y && other.z == z)
            return true;
        else
            return false;
    }
}
