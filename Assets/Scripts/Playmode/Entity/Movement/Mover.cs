using System;
using UnityEngine;

namespace Playmode.Movement
{
    public abstract class Mover : MonoBehaviour
    {
        //BEN_CORRECTION : Trois constantes, deux sortes de nommage différentes.
        public static readonly Vector3 Foward = Vector3.up;
        public static readonly Vector3 Backward = Vector3.down;
        public const float CLOCKWISE = 1f;
        
        //BEN_CORERCTION : Gros problèmes de lisibilités ici.
        [SerializeField] protected float Speed = 1f;
        [SerializeField] protected float RotateSpeed = 90f;
        private float extremeSpeed;
        public void ExtremeSpeedActivated() //BEN_CORRECTION : Nommage de la fonction. Devrait contenir un verbe.
        {
            Speed = extremeSpeed;
        }

        protected void Awake()
        {
            ValidateSerialisedFields();
            extremeSpeed = Speed * 2;
        }

        private void ValidateSerialisedFields()
        {
            if (Speed < 0)
                throw new ArgumentException("Speed can't be lower than 0.");
            if (RotateSpeed < 0)
                throw new ArgumentException("RotateSpeed can't be lower than 0.");
        }

        public abstract void Move(Vector3 direction);

        public abstract void Rotate(float direction);

        public abstract void RotateTowards(Vector3 position);
    }
}