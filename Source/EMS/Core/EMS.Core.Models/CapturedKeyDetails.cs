using System;
using EMS.Infrastructure.Common.Enums;

namespace EMS.Core.Models
{
    public class CapturedKeyDetails
    {
        public KeyboardKey KeyboardKey { get; set; }

        public DateTime CapturedOn { get; set; }
    }
}
