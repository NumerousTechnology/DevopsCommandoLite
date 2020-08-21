using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DevopsCommandoLite.Terminal.Services
{
    public class DevopsResponse<T>
    {
        public static DevopsResponse<T> Create(T data, HttpStatusCode status)
        {
            throw new NotImplementedException();
        }

        public DevopsResponse(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage ?? "";
        }

        public DevopsResponse(bool isSuccess, T data, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage ?? "";
        }

        public bool IsSuccess { get; }
        public T Data { get; }
        public string ErrorMessage { get; }
    }
}
