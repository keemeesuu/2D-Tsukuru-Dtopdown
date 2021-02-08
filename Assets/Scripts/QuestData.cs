using System.Collections;
using System.Collections.Generic;
// using UnityEngine;

public class QeustData
{
    public string questName;
    public int[] npcId;
    
    public QeustData(string name, int[] npc){
        questName = name;
        npcId = npc;
    }
}
