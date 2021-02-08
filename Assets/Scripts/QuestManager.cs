using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;
    Dictionary<int, QeustData> questList;

    void Awake(){
        // Dictionary 자료구조
        questList = new Dictionary<int, QeustData>(); 
        GenerateData();
    }

    void GenerateData(){
        questList.Add(10, new QeustData("마을 사람들과 대화하기.", new int[] { 1000, 2000 }));
        questList.Add(20, new QeustData("루도의 동전 찾아주기.", new int[] { 5000, 2000 }));
        questList.Add(30, new QeustData("퀘스트 올 클리어!", new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id){
        return questId + questActionIndex;
    }

    public string CheckQuest(int id){

        // Next Talk Target
        if(id == questList[questId].npcId[questActionIndex]){
            questActionIndex++;
        }

        // Control Quest Object
        ControlObject();

        // Talk Complete & Next Quest
        if(questActionIndex == questList[questId].npcId.Length){
            NextQuest();
        }

        // Quest Name
        return questList[questId].questName;
    }

    //  Overloading : 매개변수에 따라 함수 호출 -> CheckQuest(int id), CheckQuest()
    public string CheckQuest(){
        // Quest Name
        return questList[questId].questName;
    }

    void NextQuest(){
        questId += 10;
        questActionIndex = 0;
    }

    public void ControlObject(){
        switch(questId){
            case 10:
                if(questActionIndex == 2)
                    questObject[0].SetActive(true);
                break;
            case 20:
                if(questActionIndex == 0)
                    questObject[0].SetActive(true);
                else if(questActionIndex == 1)
                    questObject[0].SetActive(false);
                break;
        }
    }


}
