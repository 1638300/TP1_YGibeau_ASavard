using Playmode.Ennemy;
using Playmode.Util.Values;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class WorldStimulus : MonoBehaviour
    { 
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<WorldSensor>()?.Sense();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            other.GetComponent<WorldSensor>()?.Unsense();
        }
    }
}
