﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;

namespace basic_control_gesture
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal KinectRegion KinectRegion { get; set; }
    }
}
