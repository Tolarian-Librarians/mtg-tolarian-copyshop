using System;

namespace Tolarian.Copyshop.Controller
{
    public abstract class TolarianControllerBase
    {
        private string _errorMessage;

        public string GetErrorMessage()
        {
            string returnValue = _errorMessage;
            _errorMessage = string.Empty;
            return returnValue;
        }

        protected void SetErrorMessage(Exception ex)
             => _errorMessage = ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
    }
}