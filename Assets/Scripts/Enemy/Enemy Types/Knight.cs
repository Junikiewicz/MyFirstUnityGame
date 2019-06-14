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
        protected override void Attack()
        {
            theAn.SetTrigger(AnimatorSlashTrigger);
        }
    }
}
