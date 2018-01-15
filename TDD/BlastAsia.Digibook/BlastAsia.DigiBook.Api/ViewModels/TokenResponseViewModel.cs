using Newtonsoft.Json;
namespace BlastAsia.DigiBook.Api.ViewModels
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TokenResponseViewModel
    {
        #region Constructor
        public TokenResponseViewModel()
        {
        }
        #endregion

        #region Properties
        public string token { get; set; }
        public int expiration { get; set; }
        #endregion

    }
}