using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStep
{
    public int step_number;
    public string dialogue;
    public string condition_type;
    public string condition_target;
    public int condition_vaule;
}

public class NPCQuestStepList
{
    public List<QuestStep> steps;
}
