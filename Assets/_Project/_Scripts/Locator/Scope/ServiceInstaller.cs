using UnityEngine;

namespace UnityServiceLocator
{
    public abstract class ServiceInstaller : MonoBehaviour
    {
        public abstract void Install(IBuilder builder);
    }
}