using Microsoft.AspNetCore.Components;
using Soundpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Platforms.Services
{
    public class PiPService : IPiPService
    {
        public BehaviorSubject<bool> Interaction { get; set; } = new BehaviorSubject<bool>(false);

        public void Activate()
        {
            
        }
    }
}
