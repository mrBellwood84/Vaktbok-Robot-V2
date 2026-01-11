namespace WebHarvester.Harvest.Interfaces
{
    public interface ILoginBot : IBaseBot
    {
        Task RunLoginProcedureAsync();
    }
}