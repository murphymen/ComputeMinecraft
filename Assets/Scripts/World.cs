using UnityEngine;
using UnityEngine.Rendering;


public class World : UnitySingleton<World>
{
    // mesh filter and renderer
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    // World dimensions (Vector3)
    public Vector3 worldDimensions;
    public Vector3 chunkDimensions;
    public int worldVolume;
    public int chunkVolume;
    public int triangleBudget;
 
    // Reference to ChunkShaderController script.
    //ChunkShaderController chunkShaderController;

    // Mesh data
    public GraphicsBuffer vertexBuffer;
    public GraphicsBuffer normalBuffer;
    public GraphicsBuffer texcoordBuffer;
    public GraphicsBuffer indexBuffer;
    public ComputeBuffer countBuffer;
    public Mesh mesh;

    //Debug data array
    public int[] debugIndex;

    public Material material;

    // Array of all the chunks in the world
    public ChunkData[] chunks;

    // Noise generation variables
    public int seed;
    [RangeAttribute(0,1.5f)]
    public float    frequency;
    [RangeAttribute(0,1.5f)]
    public float    amplitude;
    [RangeAttribute(1, 8)]
    public int      octaves;
    [RangeAttribute(0,1.5f)]
    public float    persistance;
    [RangeAttribute(0,1.5f)]
    public float    lacunarity;



    void AllocateMesh()
    {
        mesh = new Mesh();
        mesh.vertexBufferTarget |= GraphicsBuffer.Target.Raw;
        mesh.indexBufferTarget |= GraphicsBuffer.Target.Raw;

        var vertexLayout = new[]
        {
            new VertexAttributeDescriptor(UnityEngine.Rendering.VertexAttribute.Position, 
                VertexAttributeFormat.Float32, 3, stream:0),            
            new VertexAttributeDescriptor(UnityEngine.Rendering.VertexAttribute.Normal,
                VertexAttributeFormat.Float32, 3, stream:1),
            new VertexAttributeDescriptor(UnityEngine.Rendering.VertexAttribute.TexCoord0,
                VertexAttributeFormat.Float32, 2, stream:2),
        };

        mesh.SetVertexBufferParams(triangleBudget*3, vertexLayout);
        mesh.SetIndexBufferParams(triangleBudget*3, IndexFormat.UInt32);

        mesh.SetSubMesh(0, new SubMeshDescriptor(0, triangleBudget*3, 
            MeshTopology.Triangles), MeshUpdateFlags.DontRecalculateBounds);

        vertexBuffer ??= mesh.GetVertexBuffer(0);
        normalBuffer ??= mesh.GetVertexBuffer(1);
        texcoordBuffer ??= mesh.GetVertexBuffer(2);
        indexBuffer ??= mesh.GetIndexBuffer();

        countBuffer = new ComputeBuffer(1, 4, ComputeBufferType.Counter);

        debugIndex = new int[triangleBudget*3];
   }

    void RelaseMesh()
    {
        vertexBuffer?.Release();
        normalBuffer?.Dispose();
        texcoordBuffer?.Dispose();
        indexBuffer?.Dispose();
        countBuffer.Dispose();

        Object.Destroy(mesh);
    }

    int FlatIndex(int x, int y, int z)
    {
        return (int) x + (int)(y * worldDimensions.x) + (int)(z * worldDimensions.x * worldDimensions.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Get reference to ChunkShaderController script.
        //chunkShaderController = GetComponent<ChunkShaderController>();

        worldVolume = (int) (worldDimensions.x * worldDimensions.y * worldDimensions.z);        
        chunks = new ChunkData[worldVolume];

        chunkVolume = (int) (chunkDimensions.x * chunkDimensions.y * chunkDimensions.z);

        // For each chunk in the world, create a new chunk
        for (int x = 0; x < worldDimensions.x; x++)
            for (int y = 0; y < worldDimensions.y; y++)
                for (int z = 0; z < worldDimensions.z; z++)
                {
                    // Create a new chunk
                    ChunkData newChunk = new ChunkData(new ChunkCoordinate(x,y,z));
                    chunks[FlatIndex(x, y, z)] = newChunk;

                    // Print chunk coordinates
                    Debug.Log("Chunk: " + newChunk.chunkCoordinate.x 
                        + " " + newChunk.chunkCoordinate.y 
                        + " " + newChunk.chunkCoordinate.z);
                 }    

        // Generate all the chunks
        GenerateWorld();    

        // Allocate mesh data
        AllocateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        GenerateWorld();    

        countBuffer.SetCounterValue(0);

        // For each chunk in the world
        for(int z = 0; z < worldDimensions.z; z++)
            for(int y = 0; y < worldDimensions.y; y++)
                for(int x = 0; x < worldDimensions.x; x++)
                {
                    // Render the chunk
                    ChunkShaderController.Instance.RenderChunk(chunks[FlatIndex(x, y, z)], new ChunkCoordinate(x, y, z));
                } 

        ChunkShaderController.Instance.ClearRest();

        // Assign new mesh to the mesh filter
        meshFilter.sharedMesh = mesh;

        // Calculate mesh bounds
        CalculateBounds();

        // Get data from the compute shader to debugIndex array
        indexBuffer.GetData(debugIndex);
    }

    // onDestroy is called when the object is destroyed
    void OnDestroy()
    {
        // Print debug message
        Debug.Log("Destroying world...");

        RelaseMesh();
        
        // For each chunk in the world, destroy the chunk
        for(int i = 0; i < chunks.Length; i++)
        {
            chunks[i].DisposeChunk();
        }
    }

    // Generate chunk by chunk using perlin noise
    public void GenerateWorld()
    {
        Debug.Log("Generating world...");
        // For each chunk in the world
        for(int x = 0; x < worldDimensions.x; x++)
            for(int y = 0; y < worldDimensions.y; y++)
                for(int z = 0; z < worldDimensions.z; z++)
                {
                    ChunkShaderController.Instance.GenerateVoxelMap(new ChunkCoordinate(x, y, z), chunks[FlatIndex(x, y, z)]);
                }
    }

    void CalculateBounds()
    {
        var ext = new Vector3(worldDimensions.x * chunkDimensions.x,
                              worldDimensions.y * chunkDimensions.y,
                              worldDimensions.z * chunkDimensions.z);// + new Vector3(10, 10, 10);
        mesh.bounds = new Bounds(Vector3.zero, ext);
    }
}
