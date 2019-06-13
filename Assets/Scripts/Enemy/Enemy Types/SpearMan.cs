namespace MyRPGGame.Enemies
{
    public class SpearMan : Enemy
    {
        private const string AnimatorThrustTrigger = "Thrust";
        protected override void Awake()
        {
            base.Awake();
            characterClass = EnemyClass.spearman;
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Attack()
        {
            base.Attack();
            theAn.SetTrigger(AnimatorThrustTrigger);
        }
    }
}
