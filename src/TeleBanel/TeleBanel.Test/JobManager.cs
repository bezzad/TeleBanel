using System;
using System.IO;

namespace TeleBanel.Test
{
    public class JobManager : IJobManager
    {
        public Job GetJob(string id)
        {
            return new Job()
            {
                Id = id,
                Title = "عنوان عکس " + id,
                Image = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\Resources\TestImage.JPG")
            };
        }

        public Job[] GetJobs()
        {
            throw new NotImplementedException();
        }

        public void SetJob(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
