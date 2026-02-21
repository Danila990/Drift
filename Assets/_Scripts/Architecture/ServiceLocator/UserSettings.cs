using System;
using UnityEngine;

namespace _Project
{
    [CreateAssetMenu(fileName = "UserSettings", menuName = "MySo/UserSettings")]
    public class UserSettings : ScriptableObject
    {
        [field: SerializeField] public CarConfig carConfig { get; private set; }
    }

    public enum TurretType
    {
        Default = 0,
        Fast = 1,
        Strongly = 2,
        Donate = 3,
    }

    public enum CarType
    {
        Default = 0,
        Fast = 1,
        Strongly = 2,
        Donate = 3,
    }

    [Serializable]
    public class CarConfig
    {
        public CarType CarType;
        public TurretType TurretType;
    }
}