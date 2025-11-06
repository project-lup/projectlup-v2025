using DSG.Utils.Enums;
using OpenCvSharp.Aruco;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DSG
{
    static class Operation
    {
        public static readonly Dictionary<EOperationType, Func<float, float, float>> Calc =
            new Dictionary<EOperationType, Func<float, float, float>>
            {

            };
    }
}