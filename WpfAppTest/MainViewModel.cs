using System;
using System.Collections.Generic;

namespace WpfAppTest
{
    public class MainViewModel : ObservableObject
    {
        public List<string> Collection { get; set; } = new List<string>() { "test", "test2", "qweqe" };
    }
}
