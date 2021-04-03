using UnityEngine;
using MR.Items;

public class Player : MonoBehaviour
{
    public float timer;
    Animator anim;
    SpriteRenderer rend;
    MiningController mining;
    bool facingLeft = false;

    public Sprite[] tools;
    GameObject tool;

    public SpriteRenderer holdingRend;
    public GroundItem holding = null;
    public GroundItem groundItem = null;

    // Start is called before the first frame update
    void Start()
    {
        timer = Time.realtimeSinceStartup;
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        mining = transform.GetChild(3).GetComponent<MiningController>();
        tool = transform.GetChild(1).gameObject;
        holdingRend = transform.GetChild(2).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > timer + 0.075f)
        {
            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;
            bool miningLeft = false;
            bool miningRight = false;

            if (Input.GetKey(KeyCode.Space) && holding != null && holding.item is Axe)
            {
                if (facingLeft)
                {
                    miningLeft = true;
                }
                else
                {
                    miningRight = true;
                }
                mining.SetMiningState(MiningController.MiningState.Cutting);
            }
            else
            {
                //tool.GetComponent<SpriteRenderer>().sprite = tools[0];
                mining.SetMiningState(MiningController.MiningState.None);
            }

            Vector2Int delta = Vector2Int.zero;

            if (Input.GetKey(KeyCode.W))
            {
                facingLeft = false;
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
                facingLeft = true;
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
        if (groundItem != null)
        {
            DropItem();
            holding = groundItem;
            groundItem.item.PickedUp(this);
            holding.transform.parent = transform;
            holding.gameObject.SetActive(false);
            groundItem = null;
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
}
