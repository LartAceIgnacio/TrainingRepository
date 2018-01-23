using BlastAsia.DigiBook.Domain.Pilots;

namespace BlastAsia.DigiBook.API.Controllers
{
    public class PilotsController
    {
        private IPilotService pilotService;
        private IPilotRepository pilotRepository;

        public PilotsController(IPilotService pilotService, IPilotRepository pilotRepository)
        {
            this.pilotService = pilotService;
            this.pilotRepository = pilotRepository;
        }
    }
}