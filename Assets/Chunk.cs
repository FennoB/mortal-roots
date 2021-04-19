using System.Collections.Generic;
using UnityEngine;

using MR.Changes;
using Wavescript.Gen;

public class Chunk : MonoBehaviour
{
    public GameObject[] fieldPrefabs;
    public float[] fieldProbabs;
    public Dictionary<Vector2Int, GameObject> fields = new Dictionary<Vector2Int, GameObject>();
    public int chunkWidth = 16;
    public int fieldWidth = 16;

    public void Generate()
    {
        Vector2 v = transform.position;

        // Generator
        RandomNode gen = new RandomNode(v.GetHashCode());

        for (int y = 0; y < chunkWidth; ++y)
        {
            for (int x = 0; x < chunkWidth; ++x)
            {
                RandomNode fieldGen = gen.Expand();
                int fieldtype = ProbDistribution.Categorical(fieldGen.Value, fieldProbabs);
                if (fieldtype >= 0)
                {
                    GameObject thing = Spawn(fieldPrefabs[fieldtype], x, y);
                    Vector3 r = new Vector3((float)fieldGen.Value, (float)fieldGen.Value);
                    r.z = r.y;
                    thing.transform.position += r * fieldWidth;
                    if (thing.tag == "tree")
                    {
                        thing.GetComponentInChildren<SpriteRenderer>().flipX = fieldGen.Value > 0.5;
                    }
                    else if (thing.tag == "item")
                    {
                        thing.GetComponent<WorldItem>().SetInField();
                    }
                }
            }
        }
    }

    public void ApplyChange(Change change, bool regen)
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

        if (change is TreeChopped)
        {
            GameObject obj = UnSpawn(x, y);
            Vector3 p = obj.transform.position;
            bool treeFlip = obj.GetComponentInChildren<SpriteRenderer>().flipX;
            Destroy(obj);
            obj = Spawn((change as TreeChopped).GetRoots(), x, y);
            obj.transform.position = p;
            obj.GetComponentInChildren<SpriteRenderer>().flipX = treeFlip;
        }
        else if (change is ItemTaken)
        {
            GameObject obj = UnSpawn(x, y);
            if (regen)
            {
                Destroy(obj);
            }
        }
    }

    public GameObject Spawn(GameObject prefab, int x, int y)
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(x * fieldWidth, y * fieldWidth, y * fieldWidth - 2);
        fields[new Vector2Int(x, y)] = obj;
        return obj;
    }

    public GameObject UnSpawn(int x, int y)
    {
        Vector2Int fieldPosition = new Vector2Int(x, y);
        if (fields.ContainsKey(fieldPosition))
        {
            GameObject ret = fields[fieldPosition];
            fields.Remove(fieldPosition);
            return ret;
        }
        return null;
    }
}
