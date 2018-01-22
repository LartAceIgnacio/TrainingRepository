using BlastAsia.DigiBook.Domain.Models.Names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.Api.Utils
{
    public static class NameExtensions
    {
        public static Name ApplyChanges(this Name name, Name from)
        {
            name.NameFirst = from.NameFirst;
            name.NameLast = from.NameLast;

            return name;
        }
    }
}
