using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStep
{
    public int step_number; //퀘스트번호
    public string dialogue; //intro
    public string condition_type; //
    public string condition_target; //무슨 꽃
    public int condition_vaule; //몇개
}

public class NPCQuestStepList
{
    public List<QuestStep> steps;
}
