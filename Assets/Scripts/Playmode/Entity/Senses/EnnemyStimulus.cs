using Playmode.Ennemy;
using Playmode.Util.Values;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class EnnemyStimulus : MonoBehaviour
    {
        private EnnemyController _ennemy;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _ennemy = transform.root.GetComponentInChildren<EnnemyController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<EnnemySensor>()?.Sense(_ennemy);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            other.GetComponent<EnnemySensor>()?.Unsense(_ennemy);
        }
    }
}