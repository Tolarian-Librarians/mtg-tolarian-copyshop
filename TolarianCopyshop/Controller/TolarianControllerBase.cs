using System;

namespace Tolarian.Copyshop.Controller
{
    public abstract class TolarianControllerBase
    {
        private string _errorMessage;

        public string ErrorMessage
        {
            get
            {
                string returnValue = _errorMessage;
                _errorMessage = string.Empty;
                return returnValue;
            }
            set => _errorMessage = value;
        }

        protected string BuildErrorMessage(Exception ex)
             => ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
    }
}
