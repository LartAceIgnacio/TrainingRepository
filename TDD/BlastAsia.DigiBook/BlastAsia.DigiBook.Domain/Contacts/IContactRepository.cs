﻿using BlastAsia.DigiBook.Domain.Test.Contacts.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactRepository
    {
        Contact Create(Contact contact);
    }
}
