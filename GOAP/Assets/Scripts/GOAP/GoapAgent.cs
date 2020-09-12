using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubGoal
{
    public Dictionary<string, int> subGoals;

    public bool remove;

    public SubGoal(string key, int value, bool isRemovable)
    {
        subGoals = new Dictionary<string, int>();
        subGoals.Add(key, value);

        remove = isRemovable;
    }
}

public class GoapAgent : MonoBehaviour
{
    public List<GoapAction> actions = new List<GoapAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    public GoapWorldStates beliefs = new GoapWorldStates();

    GoapPlanner planner;

    Queue<GoapAction> actionQueue;
    public GoapAction currentAction;
    public SubGoal currentGoal;

    public GoapInventory inventory = new GoapInventory();

    public void Start()
    {
        GoapAction[] objectsActions = this.GetComponents<GoapAction>();

        foreach (GoapAction action in objectsActions)
            actions.Add(action);
    }

    bool invoked = false;

    public void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();

        invoked = false;
    }

    void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, this.transform.position);

            if (currentAction.agent.hasPath && distanceToTarget < 2.0f)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }

            return;
        }

        if (planner == null || actionQueue == null)
        {
            planner = new GoapPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals) {

                actionQueue = planner.plan(actions, sg.Key.subGoals, beliefs);

                if (actionQueue != null)
                {
                    currentGoal = sg.Key;

                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
                goals.Remove(currentGoal);

            planner = null;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();

            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                if (currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
                actionQueue = null;

        }
    }
}