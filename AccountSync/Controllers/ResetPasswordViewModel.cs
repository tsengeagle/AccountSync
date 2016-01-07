using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountSync.Controllers
{
    public class ResetPasswordViewModel
    {
        public string User { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public string Name { get; set; }
    }
}
