namespace TeleBanel.Models.Middlewares
{
    public interface IJobMiddleware
    {
        Job[] GetJobs();
        string[] GetJobsId();
        Job GetJob(string id);
        void SetJob(Job job);
    }
}