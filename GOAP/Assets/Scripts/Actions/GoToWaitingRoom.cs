public class GoToWaitingRoom : GoapAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        GoapWorld.Instance.GetWorld().ModifyState("Waiting", 1);

        GoapWorld.Instance.AddPatient(this.gameObject);

        beliefs.ModifyState("atHospital", 1);

        return true;
    }
}