using UnityEngine;

public class GetPatient : GoapAction
{
    GameObject resource;

    public override bool PrePerform()
    {
        target = GoapWorld.Instance.RemovePatient();

        if (target == null)
            return false;

        resource = GoapWorld.Instance.RemoveCubicle();

        if (resource != null)
            inventory.AddItem(resource);
        else
        {
            GoapWorld.Instance.AddPatient(target);
            target = null;

            return false;
        }

        GoapWorld.Instance.GetWorld().ModifyState("FreeCubicle", -1);

        return true;
    }

    public override bool PostPerform()
    {
        GoapWorld.Instance.GetWorld().ModifyState("Waiting", -1);

        if (target)
            target.GetComponent<GoapAgent>().inventory.AddItem(resource);

        return true;
    }
}