using System;
using UnityEngine;

namespace _Project
{
    [Serializable]
    public class UserSettings
    {
       [field: SerializeField] public PlayerConfig playerConfig { get; private set; }
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
    public class PlayerConfig
    {
        public CarType CarType;
        public TurretType TurretType;
    }
}