using System;
using System.Collections;
using Playmode.Pickables.Types;
using Playmode.Util.Collections;
using UnityEngine;

namespace Playmode.Pickables
{
    public class PickableSpawner : MonoBehaviour
    {
        private PickableController[] _pickableControllers;
        [SerializeField] private float _spawnDelay = 5;
        [SerializeField] private GameObject _pickablePrefab;
        private readonly LoopingEnumerator<PickableTypes> _typeProvider = new LoopingEnumerator<PickableTypes>(_defaultPickables);
        private static PickableTypes[] _defaultPickables =
        {
            PickableTypes.Medkit,
            PickableTypes.Shotgun,
            PickableTypes.Uzi
        };

        private void Awake()
        {
            ValidateSerialisedFields();
            _pickableControllers = new PickableController[transform.childCount];
        }

        private void Start()
        {
            SpawnPickables();
        }

        private void ValidateSerialisedFields()
        {
            if (_pickablePrefab == null)
                throw new ArgumentException("Can't spawn null pickable prefab.");
            if (transform.childCount <= 0)
                throw new ArgumentException("Can't spawn enemies without spawn points. " +
                                            "Create children for this GameObject as spawn points.");
            if (_spawnDelay < 0.5f)
                throw new ArgumentException("The spawn delay must at least be of 0.5 seconds");
        }

        private void SpawnPickables()
        {

            for (var i = 0; i < transform.childCount; i++)
            { 
                _pickableControllers[i] = SpawnPickable(transform.GetChild(i).position,_typeProvider.Next());
            }
        }

        private PickableController SpawnPickable(Vector3 position, PickableTypes type)
        {
            var pickableController = Instantiate(_pickablePrefab, position, Quaternion.identity).GetComponentInChildren<PickableController>();
            pickableController.Configure(type);
            pickableController.OnDestroy += OnDestroyPickable;
            return pickableController;
        }

        private void OnDestroyPickable(PickableController pickableController)
        {
            for (int i = 0; i < _pickableControllers.Length; i++)
            {
                if (_pickableControllers[i] != null && _pickableControllers[i].Equals(pickableController))
                {
                    _pickableControllers[i] = null;
                    StartCoroutine(DelaySpawn(i));
                }
            }
        }

        private IEnumerator DelaySpawn(int index)
        {
            yield return new WaitForSeconds(_spawnDelay);
            
            _pickableControllers[index] = SpawnPickable(transform.GetChild(index).position, _typeProvider.Next());
        }

        private void OnDisable()
        {
            for (int i = 0; i < _pickableControllers.Length; i++)
            {
                if(_pickableControllers[i] != null)
                    _pickableControllers[i].OnDestroy -= OnDestroyPickable;
            }
        }
    }
}