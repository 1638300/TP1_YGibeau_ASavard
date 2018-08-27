using System.Collections.Generic;
using Playmode.Ennemy;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void WorldSensorEventHandler();

    public class WorldSensor : MonoBehaviour
    {
        public event WorldSensorEventHandler OnWorldSensed;
        public event WorldSensorEventHandler OnWorldUnsensed;

        public void Sense()
        {
            NotifyWorldSensed();
        }

        public void Unsense()
        {
            NotifyWorldSightUnsensed();
        }

        private void NotifyWorldSensed()
        {
            if (OnWorldSensed != null) OnWorldSensed();
        }

        private void NotifyWorldSightUnsensed()
        {
            if (OnWorldUnsensed != null) OnWorldUnsensed();
        }
    }
}