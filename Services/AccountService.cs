using BusinessObjects.Models;
using Microsoft.Extensions.Logging;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public AccountMember? GetAccountById(string email)
        {
            try
            {
                _logger.LogInformation("Service: Looking up account by email: {Email}", email);
                var account = _accountRepository.GetAccountById(email);
                _logger.LogInformation("Service: Account lookup result: {Found}", account != null);
                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service: Error looking up account by email: {Email}", email);
                throw;
            }
        }
    }
}
