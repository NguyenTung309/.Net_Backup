using BusinessObjects.Models;
using System;
using System.Linq;

namespace DataAccessObjects
{
    public class AccountDAO
    {
        public static AccountMember? GetAccountById(string email)
        {
            try
            {
                using var context = new MyStoreContext();
                var account = context.AccountMembers
                    .FirstOrDefault(a => a.EmailAddress == email);
                return account;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AccountDAO: {ex.Message}");
                throw;
            }
        }
    }
}