using UnityEngine;

public class Nurse : GoapAgent
{
    new void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("treatPatient", 1, false);
        goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("rested", 1, false);
        goals.Add(s2, 1);

        Invoke("GetTired", Random.Range(10.0f, 20.0f));
    }

    void GetTired() {

        beliefs.ModifyState("exhausted", 0);

        Invoke("GetTired", Random.Range(0.0f, 20.0f));
    }
}