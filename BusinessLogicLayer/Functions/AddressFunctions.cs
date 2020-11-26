using BusinessLogicLayer.Interfaces;
using BusinessObjects;
using DataAccessLayer.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Functions
{
    public class AddressFunctions : IAddress
    {

        private readonly TrackerDbContext _context = new TrackerDbContext(TrackerDbContext.ops.dbOptions);

        public async Task<Address> GetAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id);

            _context.Entry(address)
                .Reference(a => a.StateName)
                .Load();

            return address;
        }

        public async Task<Address> AddAddress(string patid, Address address)
        {
            address.StateName = null;
            if (address.StateID == 0)
                throw new Exception("State is not selected in Address. Try selecting a state ");
            address.UniqueID = patid;
            address.OccupationID = null;
            address.HospitalID = null;
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> AddAddress(Address address)
        {
            address.StateName = null;
            if (address.StateID == 0)
                throw new Exception("State is not selected in Address. Try selecting a state ");
            address.UniqueID = null;
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> UpdateAddress(int id,Address address)
        {
            Address addobj;
            using (var _datacontext = new TrackerDbContext(TrackerDbContext.ops.dbOptions))
            {
                addobj = await _datacontext.Addresses.FindAsync(id);
            }
            var uid = addobj.UniqueID;
            var oid = addobj.OccupationID;
            var hid = addobj.HospitalID;
            var addtyp = addobj.AddressType;
            address.UniqueID = uid;
            address.OccupationID = oid;
            address.HospitalID = hid;
            address.AddressType = addtyp;
            if (address.StateID == 0)
                throw new Exception("Address does not contain state. Try adding state detail in address.");
            _context.Entry(address).State = EntityState.Modified;
            var updatedaddress = await _context.Addresses.FindAsync(id);
            return updatedaddress;
        }

        public bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.ID == id);
        }
    }
}
