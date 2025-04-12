using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.Navigations
{
    public class NavigationItem
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public bool IsAuthorized { get; set; }
        public List<NavigationItem> Children { get; set; } = new List<NavigationItem>();


        public NavigationItem(string name, string caption, string url, bool isAuthorized = false)
        {
            Name = name;
            Caption = caption;
            Url = url;
            IsAuthorized = isAuthorized;

        }

        public void AddChild(NavigationItem child)
        {
            Children.Add(child);
        }
    }
}
