using BusinessObjects;
using DataAccessLayer.DataContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAddress
    {

        Task<Address> GetAddress(int id);

        Task<Address> AddAddress(string patid, Address address);

        Task<Address> AddAddress(Address address);

        Task<Address> UpdateAddress(int id, Address address);


    }
}
