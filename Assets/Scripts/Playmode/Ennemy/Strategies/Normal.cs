using Playmode.Ennemy.BodyParts;
using Playmode.Movement;

namespace Playmode.Ennemy.Strategies
{
    public class Normal : IEnnemyStrategy
    {
        private readonly Mover mover;
        private readonly HandController handController;

        public Normal(Mover mover, HandController handController)
        {
            this.mover = mover;
            this.handController = handController;
        }

        public void Act()
        {
            mover.Move(Mover.Foward);

            handController.Use();
        }
    }
}
