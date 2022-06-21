using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Services.Interfaces
{
    public interface IPiPService
    {
        void Activate();
        BehaviorSubject<bool> Interaction { get; set; }
    }
}
