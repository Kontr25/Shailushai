using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Scripts.Snail
{
    public class SnailSpawner : MonoBehaviour
    {
        [SerializeField] private int _spawnCount;
        [SerializeField] private List<Transform> _spawnPoint;
        [SerializeField] private SnailController _snailPrefab;

        public void SpawnSnails()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                int randomPoinNumber = Random.Range(0, _spawnPoint.Count);
                SnailController snail = Instantiate(_snailPrefab, _spawnPoint[randomPoinNumber].position,
                    Quaternion.identity);
                SnailCounter.Instance.AddSnailToList(snail);
                Transform snailTransform = snail.transform;
                snailTransform.SetParent(_spawnPoint[randomPoinNumber]);
                snailTransform.localRotation = Quaternion.identity;
                snailTransform.localPosition = Vector3.zero;
                _spawnPoint.Remove(_spawnPoint[randomPoinNumber]);
            }
            
            SnailCounter.Instance.SnailSpawned();
        }
    }
}