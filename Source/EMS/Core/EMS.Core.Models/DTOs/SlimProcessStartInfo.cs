using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Models.DTOs
{
    public class SlimProcessStartInfo
    {
        public string Domain { get; set; }
        public string FileName { get; set; }
        public string UserName { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public bool CreateNoWindow { get; set; }
        public bool UseShellExecute { get; set; }
    }
}
