using UnityEngine;

namespace _Scripts.UI
{
    public abstract class ObjectChanger : MonoBehaviour
    {
        [SerializeField] private ObjectChangerType ObjectChangerType;

        public ObjectChangerType ChangerType => ObjectChangerType;

        public abstract void Next();
        public abstract void Previous();
        public abstract void StartAction();
    }
}