using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.App.Services
{
    public abstract class ServiceBase
    {
        private readonly TrilhaDbContext _context;
        private ServiceError? _error;

        public ServiceError? Error
        {
            get
            {
                var retorno = _error;
                _error = null;
                return retorno;
            }
        }


        protected ServiceBase(TrilhaDbContext context)
        {
            _context = context;
        }

        //public async Task<CustomerProfile?> ValidateTenant(Guid tenant)
        //{
        //    return await _context.Set<CustomerProfile>().FirstOrDefaultAsync(x => x.Uuid == tenant);
        //}

        public bool ServiceError(ServiceError error)
        {
            _error = error;
            return false;
        }
    }

    public enum ServiceError
    {
        EmailAlreadyRegistered,
        InvalidTenant,
        InvalidToken,
        WrongUsernameOrPassword
    }
}
