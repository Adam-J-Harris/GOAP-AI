using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public Text states;

    void LateUpdate()
    {
        Dictionary<string, int> worldStates = GoapWorld.Instance.GetWorld().GetStates();

        states.text = "";

        foreach (KeyValuePair<string, int> s in worldStates)
            states.text += s.Key + ", " + s.Value + "\n";
    }
}