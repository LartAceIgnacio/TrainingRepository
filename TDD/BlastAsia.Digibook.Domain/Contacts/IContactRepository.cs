﻿using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.Digibook.Domain.Models.Contacts;

namespace BlastAsia.Digibook.Domain.Contacts
{
    public interface IContactRepository
    {
        Contact Create(Contact contact);

        Contact Retrieve(Guid id);
        Contact Update(Guid id, Contact contact);
    }
}
