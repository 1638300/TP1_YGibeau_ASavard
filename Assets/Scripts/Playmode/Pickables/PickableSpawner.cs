using System;
using Playmode.Pickables.Types;
using Playmode.Util.Collections;
using UnityEngine;

namespace Playmode.Pickables
{
    public class PickableSpawner : MonoBehaviour
    {
        private static readonly PickableTypes[] DefaultPickables =
        {
            PickableTypes.Medkit,
            PickableTypes.Shotgun,
            PickableTypes.Uzi
        };

        [SerializeField] private GameObject pickablePrefab;

        private void Awake()
        {
            ValidateSerialisedFields();
        }

        private void Start()
        {
            SpawnPickables();
        }

        private void ValidateSerialisedFields()
        {
            if (pickablePrefab == null)
                throw new ArgumentException("Can't spawn null pickable prefab.");
            if (transform.childCount <= 0)
                throw new ArgumentException("Can't spawn ennemis whitout spawn points. " +
                                            "Create childrens for this GameObject as spawn points.");
        }

        private void SpawnPickables()
        {
            var typeProvider = new LoopingEnumerator<PickableTypes>(DefaultPickables);

            for (var i = 0; i < transform.childCount; i++)
                SpawnPickable(
                    transform.GetChild(i).position,
                    typeProvider.Next()
                );
        }

        private void SpawnPickable(Vector3 position, PickableTypes type)
        {
            Instantiate(pickablePrefab, position, Quaternion.identity)
                .GetComponentInChildren<PickableController>()
                .Configure(type);
        }
    }
}