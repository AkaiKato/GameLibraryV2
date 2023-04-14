using Quartz;

namespace GameLibraryV2.Controllers
{
    public class TotalRatingCalc : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {

            return Task.CompletedTask;
        }
    }
}
