using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.ViewModels
{
    [JsonObject(MemberSerialization.OptOut)]
    public class UserViewModel
    {
        #region Constructor
        public UserViewModel()
        {
        }
        #endregion
        #region Properties
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        #endregion
    }
}
