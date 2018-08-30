using System;
using System.Collections;
using Playmode.Pickables.Types;
using Playmode.Util.Collections;
using UnityEngine;

namespace Playmode.Pickables
{
    public class PickableSpawner : MonoBehaviour
    {
        private PickableController[] pickableControllers;
        [SerializeField] private float SpawnDelay = 5;
        private LoopingEnumerator<PickableTypes> typeProvider = new LoopingEnumerator<PickableTypes>(DefaultPickables);
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
            pickableControllers = new PickableController[transform.childCount];
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
            if (SpawnDelay < 0.5f)
                throw new ArgumentException("The spawn delay must at least be of 0.5 seconds");
        }

        private void SpawnPickables()
        {

            for (var i = 0; i < transform.childCount; i++)
            { 
                pickableControllers[i] = SpawnPickable(transform.GetChild(i).position,typeProvider.Next());
            }
        }

        private PickableController SpawnPickable(Vector3 position, PickableTypes type)
        {
            Debug.Log("Spawn called");
            var pickableController = Instantiate(pickablePrefab, position, Quaternion.identity).GetComponentInChildren<PickableController>();
            pickableController.Configure(type);
            pickableController.onDestroy += OnDestroyPickable;
            return pickableController;
        }

        private void OnDestroyPickable(PickableController pickableController)
        {
            Debug.Log("DestroyCalled");
            for (int i = 0; i < pickableControllers.Length; i++)
            {
                if (pickableControllers[i] != null && pickableControllers[i].Equals(pickableController))
                {
                    Debug.Log("DelayCalled");
                    pickableControllers[i] = null;
                    StartCoroutine(DelaySpawn(i));
                }
            }
        }

        private IEnumerator DelaySpawn(int index)
        {
            Debug.Log("DelayCalled");
            yield return new WaitForSeconds(SpawnDelay);
            
            pickableControllers[index] = SpawnPickable(transform.GetChild(index).position, typeProvider.Next());
        }

        private void OnDisable()
        {
            for (int i = 0; i < pickableControllers.Length; i++)
            {
                if(pickableControllers[i] != null)
                    pickableControllers[i].onDestroy -= OnDestroyPickable;
            }
        }
    }
}