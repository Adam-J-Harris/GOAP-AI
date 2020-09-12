using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GoapAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;

    public GameObject target;
    public string targetTag;

    public float duration = 0.0f;

    public NavMeshAgent agent;

    public WorldState[] inspectablePreConditions;
    public WorldState[] inspectableEffects;

    public Dictionary<string, int> preConditions;
    public Dictionary<string, int> effects;

    public GoapWorldStates agentBeliefs;
    public GoapWorldStates beliefs;
    public GoapInventory inventory;

    public bool running = false;

    public GoapAction()
    {
        preConditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    private void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        if (inspectablePreConditions != null)
        {
            foreach (WorldState w in inspectablePreConditions)
                preConditions.Add(w.key, w.value);
        }

        if (inspectableEffects != null) {

            foreach (WorldState w in inspectableEffects)
                effects.Add(w.key, w.value);
        }

        inventory = this.GetComponent<GoapAgent>().inventory;

        beliefs = this.GetComponent<GoapAgent>().beliefs;
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAhievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> kvp in preConditions)
        {
            if (!conditions.ContainsKey(kvp.Key))
                return false;
        }

        return true;
    }

    public abstract bool PrePerform();

    public abstract bool PostPerform();
}