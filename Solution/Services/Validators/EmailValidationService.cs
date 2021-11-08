using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.UnitOfWork;
using Services.Constants;

namespace Services.Validators
{
    public class EmailValidationService
    {
        private readonly IUnitOfWork _uow;

        public EmailValidationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public string CheckEmailDuplication(string email)
        {
            var userReadRepo = _uow.GetRepository<IUserReadRepository>();

            var foundUser = userReadRepo.GetAll()
                .FirstOrDefault(x => x.Email == email.Trim());

            if (foundUser != null)
            {
                return Responses.EmailExists;
            }

            return Responses.Ok;
        }
    }
}
