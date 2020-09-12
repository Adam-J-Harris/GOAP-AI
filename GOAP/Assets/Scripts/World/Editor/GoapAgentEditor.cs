using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoapAgentVisual))]
[CanEditMultipleObjects]
public class GAgentVisualEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        GoapAgentVisual agent = (GoapAgentVisual)target;

        GUILayout.Label("Name: " + agent.name);
        GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GoapAgent>().currentAction);
        GUILayout.Label("Actions: ");

        foreach (GoapAction a in agent.gameObject.GetComponent<GoapAgent>().actions)
        {
            string pre = "";
            string eff = "";

            foreach (KeyValuePair<string, int> p in a.preConditions)
                pre += p.Key + ", ";

            foreach (KeyValuePair<string, int> e in a.effects)
                eff += e.Key + ", ";

            GUILayout.Label("====  " + a.actionName + "(" + pre + ")(" + eff + ")");
        }

        GUILayout.Label("Goals: ");

        foreach (KeyValuePair<SubGoal, int> g in agent.gameObject.GetComponent<GoapAgent>().goals)
        {
            GUILayout.Label("---: ");

            foreach (KeyValuePair<string, int> sg in g.Key.subGoals)
                GUILayout.Label("=====  " + sg.Key);
        }

        GUILayout.Label("Beliefs: ");

        foreach (KeyValuePair<string, int> sg in agent.gameObject.GetComponent<GoapAgent>().beliefs.GetStates())
            GUILayout.Label("=====  " + sg.Key);

        GUILayout.Label("Inventory: ");

        foreach (GameObject g in agent.gameObject.GetComponent<GoapAgent>().inventory.items)
            GUILayout.Label("====  " + g.tag);

        serializedObject.ApplyModifiedProperties();
    }
}