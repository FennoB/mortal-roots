using System.Collections.Generic;
using UnityEngine;

using MR.Changes;

public class Chunk : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public float[] treeProbabs;
    public Dictionary<Vector2Int, GameObject> fields = new Dictionary<Vector2Int, GameObject>();
    public int chunkWidth = 16;
    public int fieldWidth = 16;

    // Start is called before the first frame update
    public void Generate()
    {
        Vector2 v = transform.position;

        // Generator
        Random.InitState(v.GetHashCode());

        for (int y = 0; y < chunkWidth; ++y)
        {
            for (int x = 0; x < chunkWidth; ++x)
            {
                for (int i = 0; i < treePrefabs.Length; ++i)
                {
                    GameObject prefab = treePrefabs[i];
                    float probab = treeProbabs[i];
                    if (probab > Random.value)
                    {
                        GameObject field = Instantiate(prefab);
                        field.transform.SetParent(transform);
                        field.transform.localPosition = new Vector3(x * fieldWidth, y * fieldWidth, y * fieldWidth - 2);
                        field.GetComponentInChildren<SpriteRenderer>().flipX = Random.value > 0.5;
                        fields[new Vector2Int(x, y)] = field;
                        break;
                    }
                }
            }
        }
    }

    public void ApplyChange(Change change)
    {
        if (change is TreeChopped)
        {
            int x = (Mathf.FloorToInt(change.Where.x / fieldWidth)) % chunkWidth;
            if (x < 0)
            {
                x += chunkWidth;
            }
            int y = (Mathf.FloorToInt(change.Where.y / fieldWidth)) % chunkWidth;
            if (y < 0)
            {
                y += chunkWidth;
            }
            Vector2Int fieldPosition = new Vector2Int(x, y);
            if (fields.ContainsKey(fieldPosition))
            {
                Destroy(fields[fieldPosition]);
            }
        }
    }
}
