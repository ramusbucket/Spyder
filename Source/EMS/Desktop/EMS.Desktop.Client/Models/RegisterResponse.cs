using System.Collections.Generic;

namespace EMS.Desktop.Client.Models
{
    public class RegisterResponse
    {
        public string Message { get; set; }

        public List<string> ModelState { get; set; }
    }
}
