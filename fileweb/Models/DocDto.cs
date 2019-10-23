using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fileweb.Models
{
    public class DocDto
    {
        public int Id { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public string Category4 { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool NewWindow { get; set; }
        public bool Visible  { get; set; }
        public string Icon { get; set; }
    }
}
