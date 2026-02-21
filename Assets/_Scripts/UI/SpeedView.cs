using _Project.UnityServiceLocator;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Project
{
    public class SpeedView : MonoBehaviour
    {
        [SerializeField] private Text _speedText;

        [Inject] private CarEngine _car;


        private void Start()
        {
            ServiceLocator.Inject(this);

            if (_car != null)
                StartCoroutine(SpeedTick());
        }

        private IEnumerator SpeedTick()
        {
            while(true)
            {
                _speedText.text = _car.Speed.ToString();
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
