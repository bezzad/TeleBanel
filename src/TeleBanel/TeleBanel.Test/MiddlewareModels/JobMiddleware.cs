using System;
using System.IO;
using TeleBanel.Models;
using TeleBanel.Models.Middlewares;

namespace TeleBanel.Test.MiddlewareModels
{
    public class JobMiddleware : IJobMiddleware
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

        public string[] GetJobsId()
        {
            throw new NotImplementedException();
        }

        public void SetJob(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
