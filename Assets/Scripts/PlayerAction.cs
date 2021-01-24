using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{

    float h;
    float v;
    bool isHorizonMove;
    public float speed;
    Rigidbody2D rigid;
    Animator anime;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move Value
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // Check Button Down & Up
        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");

        // Check Horizontal Move
        if(hDown || vUp){
            // Debug.Log("가로 이어서 이동");
            isHorizonMove = true;
        }else if(vDown || hUp){
            // Debug.Log("세로 이어서 이동");
            isHorizonMove = false;
        }
        
        if(hDown){
            Debug.Log("h : " + h);
            Debug.Log("hDown : " + hDown);
            Debug.Log("hAxisRaw : " + anime.GetFloat("hAxisRaw"));
        }

        /*/ Animation
        if(anime.GetFloat("hAxisRaw") != h){
            anime.SetBool("isChange", true);
            anime.SetFloat("hAxisRaw", h);
        }else if(anime.GetFloat("vAxisRaw") != v){
            anime.SetBool("isChange", true);
            anime.SetFloat("vAxisRaw", v);
        }else{
            anime.SetBool("isChange", false);
        }
        */

    }

    void FixedUpdate(){
        // Move
        // 삼항연산자
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * speed;
    }
}
