public interface IBattleable
{
    public void TakeDamage(float damage);

    public bool IsDead();

    public string ChangeMode();

    public void PlayTurn();
}
