using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GoapAction action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, GoapAction action)
    {
        this.parent = parent;
        this.cost = cost;

        this.state = new Dictionary<string, int>(allStates);

        this.action = action;
    }

    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, GoapAction action)
    {
        this.parent = parent;
        this.cost = cost;

        this.state = new Dictionary<string, int>(allStates);

        foreach (KeyValuePair<string, int> b in beliefStates)
        {
            if (!this.state.ContainsKey(b.Key))
                this.state.Add(b.Key, b.Value);
        }

        this.action = action;
    }
}

public class GoapPlanner
{
    public Queue<GoapAction> plan(List<GoapAction> actions, Dictionary<string, int> goal, GoapWorldStates beliefStates) {

        List<GoapAction> usableActions = new List<GoapAction>();

        foreach (GoapAction a in actions)
        {
            if (a.IsAchievable())
                usableActions.Add(a);
        }

        List<Node> leaves = new List<Node>();

        Node start = new Node(null, 0.0f, GoapWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
            return null;

        Node cheapest = null;

        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else if (leaf.cost < cheapest.cost)
                cheapest = leaf;
        }

        List<GoapAction> result = new List<GoapAction>();

        Node node = cheapest;

        while (node != null)
        {
            if (node.action != null)
                result.Insert(0, node.action);

            node = node.parent;
        }

        Queue<GoapAction> queue = new Queue<GoapAction>();

        foreach (GoapAction action in result)
            queue.Enqueue(action);

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GoapAction> usableActions, Dictionary<string, int> goal) {

        bool foundPath = false;

        foreach (GoapAction action in usableActions)
        {
            if (action.IsAhievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);

                foreach (KeyValuePair<string, int> effect in action.effects)
                {
                    if (!currentState.ContainsKey(effect.Key))
                        currentState.Add(effect.Key, effect.Value);
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);

                    foundPath = true;
                }
                else
                {
                    List<GoapAction> subset = ActionSubset(usableActions, action);

                    bool found = BuildGraph(node, leaves, subset, goal);

                    if (found)
                        foundPath = true;
                }
            }
        }

        return foundPath;
    }

    private List<GoapAction> ActionSubset(List<GoapAction> actions, GoapAction actionToRemove)
    {
        List<GoapAction> subset = new List<GoapAction>();

        foreach (GoapAction action in actions)
        {
            if (!action.Equals(actionToRemove))
                subset.Add(action);
        }
        return subset;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
                return false;
        }

        return true;
    }
}