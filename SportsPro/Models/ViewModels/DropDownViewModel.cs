using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class DropDownViewModel
    {
        public Dictionary<string, string> Items { get; set; }
        public string SelectedValue { get; set; }
        public string DefaultValue { get; set; }
        
        public string DefaultText { get; set; }

        public bool HasDefault => !string.IsNullOrEmpty(DefaultText);

    }
}
