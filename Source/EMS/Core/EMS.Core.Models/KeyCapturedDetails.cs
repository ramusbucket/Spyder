using System;
using EMS.Infrastructure.Common.Enums;

namespace EMS.Core.Models
{
    public class KeyCapturedDetails
    {
        public KeyboardKey KeyboardKey { get; set; }

        public DateTime CapturedOn { get; set; }
    }
}
