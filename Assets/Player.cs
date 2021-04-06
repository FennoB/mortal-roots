using UnityEngine;
using MR.Items;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float timer;
    Animator anim;
    SpriteRenderer rend;
    public MiningController mining;
    bool facingLeft = false;

    public Sprite[] tools;
    GameObject tool;

    public float[] itemProbabilities;
    public GameObject[] itemPrefabs;

    public SpriteRenderer holdingRend;
    public WorldItem holding = null;
    public WorldItem worldItem = null;

    public HealthSystem health;

    // Start is called before the first frame update
    void Start()
    {
        timer = Time.realtimeSinceStartup;
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        mining = transform.GetChild(3).GetComponent<MiningController>();
        tool = transform.GetChild(1).gameObject;
        holdingRend = transform.GetChild(2).GetComponent<SpriteRenderer>();
        health = GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!health.alive)
        {
            if (Input.GetKey("r"))
            {
                SceneManager.LoadScene("main");
            }
            return;
        }
        if (Time.realtimeSinceStartup > timer + 0.075f)
        {
            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;
            bool miningLeft = false;
            bool miningRight = false;

            if (!DialogueWriter.main.writing &&
                Input.GetKey(KeyCode.Space))
            {
                if (holding != null)
                {
                    holding.item.Used();
                }
                else
                {
                    SearchItems();
                }
            }
            else
            {
                mining.SetMiningState(MiningController.MiningState.None);
            }

            if (mining.currentState != MiningController.MiningState.None)
            {
                if (facingLeft)
                {
                    miningLeft = true;
                }
                else
                {
                    miningRight = true;
                }
            }


            Vector2Int delta = Vector2Int.zero;

            if (Input.GetKey(KeyCode.W))
            {
                delta.y = 1;
                up = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                facingLeft = true;
                delta.x = -1;
                left = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                delta.y = -1;
                down = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                facingLeft = false;
                delta.x = 1;
                right = true;
            }

            anim.SetBool("WalkUp", up);
            anim.SetBool("WalkDown", down);
            anim.SetBool("WalkLeft", left);
            anim.SetBool("WalkRight", right);
            anim.SetBool("MineRight", miningRight);
            anim.SetBool("MineLeft", miningLeft);

            if (holding != null)
            {
                holdingRend.sprite = holding.sprites[down ? 0 : 1];
            }
            else
            {
                holdingRend.sprite = null;
            }

            Vector2Int nextPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) + delta;
            GetComponent<Rigidbody2D>().MovePosition(nextPos);
            //transform.position += delta;
            timer = Time.realtimeSinceStartup;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeItem();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    public void TakeItem()
    {
        if (worldItem != null)
        {
            DropItem();
            holding = worldItem;
            worldItem.item.PickedUp(this);
            holding.transform.parent = transform;
            holding.gameObject.SetActive(false);
            worldItem = null;
        }
    }

    public void DropItem()
    {
        if (holding != null)
        {
            holding.gameObject.SetActive(true);
            holding.transform.parent = null;
            holding.transform.position = transform.position + new Vector3(0, 0, 1);
            holding = null;
        }
    }

    public void SearchItems()
    {
        float r = Random.value;
        GameObject prefab = null;

        for (int i = 0; i < itemPrefabs.Length; ++i)
        {
            if (r < itemProbabilities[i])
            {
                prefab = itemPrefabs[i];
                break;
            }
            r -= itemProbabilities[i];
        }

        if (prefab != null)
        {
            GameObject item = GameObject.Instantiate(prefab);
            worldItem = item.GetComponent<WorldItem>();
            worldItem.item.Info();
            TakeItem();
        }
        else
        {
            Dialogue.Create()
                        .Sentence("Nothing to find here")
                        .Show();
        }
    }
}
