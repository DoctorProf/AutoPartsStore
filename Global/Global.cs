using AutoPartsStore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoPartsStore.Global
{
    internal class Global
    {
        public static User CurrentUser { get; set; }
        public static Frame CurrentFrame { get; set; }
    }
}
