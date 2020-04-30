using System;

namespace DotNet2020.Domain._5.Services
{
    public class RequestResult<T>
    {
        public RequestResult(T result = default, bool isSuccess = true, Exception error = null)
        {
            Result = result;
            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Result
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        /// Is operation completed successfully
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Error
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Safely execute action
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns></returns>
        public static RequestResult<T> SaveExecute(Action action)
        {
            var result = new RequestResult<T>();
            try
            {
                action();
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Error = e;
            }
            return result;
        }

        /// <summary>
        /// Safely execute function
        /// </summary>
        /// <param name="action">Function</param>
        /// <returns></returns>
        public static RequestResult<T> SaveExecute(Func<T> func)
        {
            var result = new RequestResult<T>();
            try
            {
                result.Result = func();
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Error = e;
            }
            return result;
        }
    }

    public class RequestResult
    {
        public RequestResult(bool isSuccess = true, Exception error = null)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Is operation completed successfully
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Error
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Safely execute action
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns></returns>
        public static RequestResult SaveExecute(Action action)
        {
            var result = new RequestResult();
            try
            {
                action();
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Error = e;
            }
            return result;
        }
    }
}
