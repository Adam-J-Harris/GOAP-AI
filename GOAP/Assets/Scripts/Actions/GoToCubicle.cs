public class GoToCubicle : GoapAction
{
    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");

        if (target == null)
            return false;

        return true;
    }

    public override bool PostPerform() {

        GoapWorld.Instance.GetWorld().ModifyState("TreatingPatient", 1);
        GoapWorld.Instance.GetWorld().ModifyState("FreeCubicle", 1);

        GoapWorld.Instance.AddCubicle(target);

        inventory.RemoveItem(target);

        return true;
    }
}