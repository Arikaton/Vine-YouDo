using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class ResetableScroll : MonoBehaviour
    {
        public enum Manager {AddVineManager, GetVineManager}

        public Manager manager;
        private ScrollRect scrollView;

        private void Start()
        {
            scrollView = GetComponent<ScrollRect>();
        }

        private void OnEnable()
        {
            if (manager == Manager.AddVineManager)
            {
                AddVineManager.Main.OnReset += Reset;
            }
            else
            {
                GetVineManager.main.OnReset += Reset;
            }
        }

        private void OnDestroy()
        {
            if (manager == Manager.AddVineManager)
            {
                AddVineManager.Main.OnReset -= Reset;
            }
            else
            {
                GetVineManager.main.OnReset -= Reset;
            }
        }

        public void Reset()
        {
            Debug.Log("Scroll reset");
            scrollView.verticalNormalizedPosition = 1;
        }
    }
}