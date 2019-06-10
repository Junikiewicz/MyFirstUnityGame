namespace MyRPGGame.Enemies
{
    public class Knight : Enemy
    {
        protected override void Awake()
        {
            base.Awake();
            characterClass = EnemyClass.knight;
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Attack()
        {
            base.Attack();
            theAn.SetTrigger("Slash");
        }
    }
}
