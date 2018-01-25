using BlastAsia.DigiBook.Domain.Models.Luigis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class LuigiExtension
    {
        public static Luigi ApplyChanges(
            this Luigi luigi,
            Luigi form)
        {
            luigi.FirstName = form.FirstName;
            luigi.LastName = form.LastName;

            return luigi;
        }
    }
}
