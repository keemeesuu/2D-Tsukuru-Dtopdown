using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public Animator talkPanel;
    public Animator portraitAnim;
    public TypeEffect talk;
    public Text questTalk;
    public GameObject menuSet;
    public Image portraitImg;
    public Sprite prevPortait;
    public GameObject scanObject;
    public GameObject player;
    public bool isAction;
    public int talkIndex;

    void Start(){
        GameLoad();
        // Debug.Log(questManager.CheckQuest());
        questTalk.text = questManager.CheckQuest();
    }

    void Update(){
        // Sub Menu
        if(Input.GetButtonDown("Cancel")){
            SubMenuActive();
        }
    }

    public void SubMenuActive(){
        if(menuSet.activeSelf)
            menuSet.SetActive(false);
        else
            menuSet.SetActive(true);
    }

    public void Action(GameObject scanObj){
        // Get Current Object
        scanObject = scanObj;
        Objdata objdata = scanObject.GetComponent<Objdata>();
        Talk(objdata.id, objdata.isNpc);
        
        // Visible Talk for Action
        talkPanel.SetBool("isShow", isAction);
    }

    void Talk(int id, bool isNpc){
        // Set Talk Data
        int questTalkIndex = 0;
        string talkData = "";

        if(talk.isAnim){
            talk.SetMsg("");
            return;
        }else{
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        // End Talk
        if(talkData == null){
            isAction = false;
            talkIndex = 0;
            // Debug.Log(questManager.CheckQuest(id));
            questTalk.text = questManager.CheckQuest(id);
            return;
        }

        // Continue Talk
        if(isNpc){
            // Split() : 구분자를 통하여 배열로 나눠주는 문자열 함수
            talk.SetMsg(talkData.Split(':')[0]);

            // Parse() : 문자열을 해당 타입으로 변환해주는 함수 (형변환)
            // Show Portrait
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
            // Animation Portrait
            if(prevPortait != portraitImg.sprite){
                portraitAnim.SetTrigger("doEffect");
                prevPortait = portraitImg.sprite;
            }
        }else{
            talk.SetMsg(talkData);

            // Hide Portrait
            portraitImg.color = new Color(1, 1, 1, 0);
        }

        // Next Talk
        isAction = true;
        talkIndex++;
    }

    public void GameSave(){
        // PlayerPrefs : 간단한 데이터 저장 기능을 지원하는 클래스
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }
    
    public void GameLoad(){
        if(!PlayerPrefs.HasKey("PlayerX"))
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        questManager.ControlObject();
    }

    public void GameExit(){
        Application.Quit();
    }

}
