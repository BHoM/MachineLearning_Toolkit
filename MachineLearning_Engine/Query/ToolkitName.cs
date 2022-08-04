﻿using BH.oM.Base.Attributes;

using System.ComponentModel;

namespace BH.Engine.MachineLearning
{
    public static partial class Query
    {
        [Description("Get the name of the current toolkit.")]
        [Output("name", "The name of the current toolkit.")]
        public static string ToolkitName()
        {
            return "MachineLearning_Toolkit";
        }
    }
}
