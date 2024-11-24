
public abstract class BaseState 
{
    public abstract void OnEnter(Enemy enemy);
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void OnExit();
    protected Enemy currentEnemy;

}
