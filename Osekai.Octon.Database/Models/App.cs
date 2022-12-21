using System;
using System.Collections.Generic;
using Osekai.Octon.Database.Models;

namespace Osekai.Octon.Database.EntityFramework
{
    public sealed class App
    {
        public App()
        {
            AppThemes = new HashSet<AppTheme>();
        }

        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; } = null!;
        public string SimpleName { get; set; } = null!;
        public bool Visible { get; set; }
        public bool Experimental { get; set; }

        public ICollection<AppTheme> AppThemes { get; set; }
    }
}
