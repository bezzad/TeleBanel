namespace TeleBanel.Models
{
    public interface IJob
    {
        Job[] GetJobs();
        string[] GetJobsId();
        Job GetJob(string id);
        void SetJob(Job job);
    }
}