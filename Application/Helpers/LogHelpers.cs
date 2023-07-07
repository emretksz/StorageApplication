using Application.Constants;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Entities.Concrete;

namespace StorageApplication.Helpers
{
    public class LogHelpers
    {
        readonly ILoggerDal _loggerRepository;
        readonly IBaseLoggerDal _baseLoggerRepository;

        public LogHelpers(ILoggerDal loggerRepository, IBaseLoggerDal baseLoggerRepository)
        {
            _loggerRepository = loggerRepository;
            _baseLoggerRepository = baseLoggerRepository;
        }

        public async Task<int> CreateLog(LogModel model,int baseLogId)
        {
            model.BaseLoggerId = Convert.ToInt32(baseLogId);
            var id = await _loggerRepository.CreateAsyncReturnId(model);
            return Convert.ToInt32(id);
        }
    }
}
