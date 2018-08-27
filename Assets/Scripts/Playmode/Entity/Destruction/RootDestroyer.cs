using System.Collections;
using UnityEngine;

namespace Playmode.Entity.Destruction
{
    public class RootDestroyer : Destroyer
    {
        public override void Destroy()
        {
            StartCoroutine(DelayDestroy());
        }

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForEndOfFrame();
            Destroy(transform.root.gameObject);
        }
    }
}