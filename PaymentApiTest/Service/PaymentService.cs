using Microsoft.EntityFrameworkCore;
using PaymentApiTest.Data;
using PaymentApiTest.Models;
using System;
using System.Linq;

namespace PaymentApi.Service
{
    public class PaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public bool MakePayment(int userId)
        {
            var user = _context.Users.Include(u => u.Transactions).SingleOrDefault(u => u.Id == userId);

            if (user == null || user.Balance < 1.1m)
            {
                return false;
            }

            if (user != null)
            {
                user.Balance -= 1.1m;
                user.Transactions.Add(new Transaction { Amount = 1.1m, Timestamp = DateTime.Now });
                _context.SaveChanges();
                return true;
            }

            return false;

        }
    }
}
