using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Menus
{
    public class WarningAuto : MonoBehaviour
    {
        private const float Delay = 7;
        private void Start()
        {
            Invoke(nameof(ChangeScene), Delay);
        }

        private void ChangeScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
