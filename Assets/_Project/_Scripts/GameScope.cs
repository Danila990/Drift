using System;
using UnityEngine;
using UnityServiceLocator;

namespace _Project
{
    public class GameScope : ServiceScope
    {
        [Header("Настройки Scope")]
        [SerializeField] private PlayerFactoryInfo _playerFactoryInfo;
        [SerializeField] private PlayerConfig _playerConfig;

        public override void Configurate(IBuilder builder)
        {
            BuildPlayer(builder);
        }

        private void BuildPlayer(IBuilder builder)
        {
            Car car = _playerFactoryInfo.CreateCar(_playerConfig);

            builder.Register(car);
        }
    }

    [Serializable]
    public class PlayerConfig
    {
        public CarType CarType;
        public TurretType TurretType;
    }
}