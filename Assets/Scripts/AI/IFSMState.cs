//Interface for Chick States

public interface IFSMState
{
    public FSMStateType stateName { get; }

    public void onEnter();
    public void onExit();
    public void doAction();
    public FSMStateType shouldTransitionToState();
}