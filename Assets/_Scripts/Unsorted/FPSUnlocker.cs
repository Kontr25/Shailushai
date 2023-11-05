﻿using UnityEngine;

namespace _Scripts.TechnicalScripts
{
    public class FPSUnlocker : MonoBehaviour
    {
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}