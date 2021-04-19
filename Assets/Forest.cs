using System.Collections.Generic;
using UnityEngine;
using MR.Changes;

public class Forest : MonoBehaviour
{
    public GameObject chunkPrefab;
    public int renderDistance = 10;
    public int chunkWidth = 16;
    public int fieldWidth = 8;
    public Dictionary<Vector2Int, Chunk> chunks;
    public Dictionary<Vector2Int, List<Change>> chunkChanges;
    public Vector2Int lastPlayerChunk;

    public void Start()
    {
        chunks = new Dictionary<Vector2Int, Chunk>();
        UpdateChunks(0, 0);
        lastPlayerChunk = new Vector2Int(0, 0);
    }

    public void UpdateChunks(int px, int py)
    {        
        if (chunkChanges == null)
        {
            chunkChanges = new Dictionary<Vector2Int, List<Change>>();
        }

        Dictionary<Vector2Int, Chunk> oldChunks = chunks;
        chunks = new Dictionary<Vector2Int, Chunk>();
        for (int x = px - renderDistance; x < px + renderDistance; ++x)
        {
            for (int y = py - renderDistance; y < py + renderDistance; ++y)
            {
                Chunk c = null;
                Vector2Int key = new Vector2Int(x, y);
                if (oldChunks.ContainsKey(key))
                {
                    c = oldChunks[key];
                    oldChunks.Remove(key);
                }
                else
                {
                    GameObject chunk = Instantiate(chunkPrefab);
                    chunk.transform.position = new Vector3(x * chunkWidth * fieldWidth, y * chunkWidth * fieldWidth, y * fieldWidth * chunkWidth);
                    chunk.transform.parent = transform;
                    c = chunk.GetComponent<Chunk>();
                    c.chunkWidth = chunkWidth;
                    c.fieldWidth = fieldWidth;
                    c.Generate();
                    if (chunkChanges.ContainsKey(key))
                    {
                        foreach (Change change in chunkChanges[key])
                        {
                            c.ApplyChange(change, true);
                        }
                    }
                }
                chunks[key] = c;
            }
        }

        foreach(Chunk c in oldChunks.Values)
        {
            Destroy(c.gameObject);
        }
    }

    public void FixedUpdate()
    {
        Vector2 playerPos = Camera.main.transform.position;
        Vector2Int playerChunk = new Vector2Int(Mathf.RoundToInt(playerPos.x / chunkWidth / fieldWidth), Mathf.RoundToInt(playerPos.y / chunkWidth / fieldWidth));
        if (playerChunk != lastPlayerChunk)
        {
            UpdateChunks(playerChunk.x, playerChunk.y);
            lastPlayerChunk = playerChunk;
        }
    }

    public void AddChange(Change change)
    {
        Vector2 location = change.Where;
        int x = Mathf.FloorToInt(location.x / fieldWidth / chunkWidth);
        int y = Mathf.FloorToInt(location.y / fieldWidth / chunkWidth);
        Vector2Int chunk = new Vector2Int(x, y);

        if (!chunkChanges.ContainsKey(chunk))
        {
            chunkChanges[chunk] = new List<Change>();
        }
        chunkChanges[chunk].Add(change);
        chunks[chunk].ApplyChange(change, false);
    }
}
