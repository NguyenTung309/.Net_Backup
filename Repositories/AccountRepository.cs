using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.Extensions.Logging;
using System;

namespace Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(ILogger<AccountRepository> logger)
        {
            _logger = logger;
        }

        public AccountMember? GetAccountById(string email)
        {
            try
            {
                _logger.LogInformation("Repository: Looking up account by email: {Email}", email);

                // Gọi phương thức static từ AccountDAO
                var account = AccountDAO.GetAccountById(email);

                _logger.LogInformation("Repository: Account lookup result: {Found}", account != null);
                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: Error looking up account by email: {Email}", email);
                throw;
            }
        }
    }
}