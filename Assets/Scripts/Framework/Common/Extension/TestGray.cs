using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace UI
{
    public class TestGray : MonoBehaviour
    {
        public bool Gray;

        private void OnValidate()
        {
            gameObject.SetGray(Gray);
        }
    }
}
