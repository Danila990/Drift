using _Project.UnityServiceLocator;
using UnityEngine;

namespace _Project
{
    public class GameServiceLocator : ServiceLocator
    {
        [Space(5), Header("Game Service Locator")]
        [SerializeField] private EnemySpawner _enemySpawner;

        public override void SetupSettings(UserSettings userSettings)
        {
            
        }

        public override void Configurate(IBuilder builder)
        {
            BuidPlayer(builder);
            //builder.Register(_enemySpawner);
        }

        private void BuidPlayer(IBuilder builder)
        {
            CarFactory carFactory = _resources.Get<CarFactory>();
            CarConfig carConfig = _settings.carConfig;

            Car car = carFactory.CreateCar(carConfig);
            builder.Register(car);
            builder.Register(car.GetComponent<CarEngine>());
        }
    }
}