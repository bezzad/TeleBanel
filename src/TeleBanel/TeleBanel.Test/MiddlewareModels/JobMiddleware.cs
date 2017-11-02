using System;
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
                Image = Properties.Resources.TestImage.ToByte()
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
