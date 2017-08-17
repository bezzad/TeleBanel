using System;
using System.Collections.Generic;
using System.Text;

namespace TeleBanel
{
    public interface IJobManager
    {
        Job[] GetJobs();
        Job GetJob(string id);
        void SetJob(Job job);
    }
}
