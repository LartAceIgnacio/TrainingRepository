namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService
    {
        private IPilotRepository @object;

        public PilotService(IPilotRepository @object)
        {
            this.@object = @object;
        }
    }
}