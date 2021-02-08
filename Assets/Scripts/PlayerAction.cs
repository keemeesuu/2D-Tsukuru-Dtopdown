using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public GameManager manager;
    public float speed;
    float h;
    float v;
    bool hDown, vDown, hUp, vUp;

    bool isHorizonMove;
    Rigidbody2D rigid;
    Animator anime;
    Vector3 dirVec;
    GameObject scanObject;

    // Mobile Key Var
    int up_Value, down_Value, left_Value, right_Value;
    bool up_Down, down_Down, left_Down, right_Down;
    bool up_Up, down_Up, left_Up, right_Up;
 

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move Value(PC+Mobile)
        h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal") + right_Value + left_Value; // 더하는 이유는 움직임 상쇄 때문에(동시에 누르면 1 -1 = 0)
        v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical") + up_Value + down_Value;;
        

        // Check Button Down & Up(PC+Mobile)
        hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal") || right_Down || left_Down ;
        vDown = manager.isAction ? false : Input.GetButtonDown("Vertical") || up_Down || down_Down;
        hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal") || right_Up || left_Up;
        vUp = manager.isAction ? false : Input.GetButtonUp("Vertical") || up_Up || down_Up;


        // Check Horizontal Move
        if(hDown || vUp){
            // Debug.Log("가로 이어서 이동");
            isHorizonMove = true;
        }else if(vDown || hUp){
            // Debug.Log("세로 이어서 이동");
            isHorizonMove = false;
        }else if(hUp || vUp){
            // 현재 AxisRaw 값에 따라 수평, 수직 판단하여 해결
            isHorizonMove = h != 0;
        }

        // Animation
        if(anime.GetFloat("hAxisRaw") != h){
            anime.SetBool("isChange", true);
            anime.SetFloat("hAxisRaw", h);
        }else if(anime.GetFloat("vAxisRaw") != v){
            anime.SetBool("isChange", true);
            anime.SetFloat("vAxisRaw", v);
        }else{
            anime.SetBool("isChange", false);
        }

        // Direction
        if(vDown && v == 1){
            dirVec = Vector3.up;
        }else if(vDown && v == -1){
            dirVec = Vector3.down;
        }else if(hDown && h == 1){
            dirVec = Vector3.right;
        }else if(hDown && h == -1){
            dirVec = Vector3.left;
        }

        // Scan Object
        if(Input.GetButtonDown("Jump") && scanObject != null){
            // Debug.Log("this is : " + scanObject.name);
            manager.Action(scanObject);
        }

        // Mobile Var Init - 초기화 작업
        up_Down = false;
        down_Down = false;
        left_Down = false;
        right_Down = false;
        up_Up = false;
        down_Up = false;
        left_Up = false;
        right_Up = false;

    }

    void FixedUpdate(){

        // Move
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * speed;

        // Ray
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if(rayHit.collider != null){
            scanObject = rayHit.collider.gameObject;
        }else{
            scanObject = null;
        }

    }

    public void ButtonDown(string type){
        switch(type){
            case "U":
                up_Value = 1;
                up_Down = true;
                break;
            case "D":
                down_Value = -1;
                down_Down = true;
                break;
            case "L":
                left_Value = -1;
                left_Down = true;
                break;
            case "R":
                right_Value = 1;
                right_Down = true;
                break;
            case "A":
                // Scan Object
                if(scanObject != null)
                    manager.Action(scanObject);
                break;
            case "C":
                manager.SubMenuActive();
                break;
        }
    }
    public void ButtonUp(string type){
        switch(type){
            case "U":
                up_Value = 0;
                up_Up = true;
                break;
            case "D":
                down_Value = 0;
                down_Up = true;
                break;
            case "L":
                left_Value = 0;
                left_Up = true;
                break;
            case "R":
                right_Value = 0;
                right_Up = true;
                break;
        }
    }

}
