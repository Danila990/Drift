using _Project.Bootstrap;
using UnityEngine;

namespace _Project
{
    public class GameScope : BootstrapScope
    {
        [Space(5), Header("Game Scope Settings")]
        [SerializeField] private UserSettings _settings;
        [SerializeField] private UserFactory _userFactory;

        [SerializeField] private EnemySpawner _enemySpawner;

        public override void Configurate(IBuilder builder)
        {
            builder.Register(_settings);
            BuidPlayer(builder);
            builder.Register(_enemySpawner);
        }

        private void BuidPlayer(IBuilder builder)
        {
            Car car = _userFactory.CreateCar(_settings.playerConfig);
            builder.Register(car);
            builder.Register(car.GetComponent<CarEngine>());
        }
    }
}