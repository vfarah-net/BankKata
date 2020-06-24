using System;

namespace Codurance.Domain
{
    public class Clock : IClock
    {        
        public string Now
        {
            get { return DateTime.Now.ToString("dd/mm/yyyy"); }
        }
    }
}
