using System;
using System.Collections.Generic;

namespace Osekai.Octon.Database.Models
{
    public sealed class HomeFaq
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string LocalizationPrefix { get; set; } = null!;
    }
}
