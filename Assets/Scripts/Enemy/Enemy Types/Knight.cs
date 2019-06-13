namespace MyRPGGame.Enemies
{
    public class Knight : Enemy
    {
        private const string AnimatorSlashTrigger = "Slash";
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
            theAn.SetTrigger(AnimatorSlashTrigger);
        }
    }
}
