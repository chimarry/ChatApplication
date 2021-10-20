namespace ChatApplication.WebAPI.Models
{
    public class RegisterUserResponseWrapper
    {
        /// <summary>
        /// Is the image for the event changed?
        /// </summary>
        public bool IsRegistered { get; }

        public RegisterUserResponseWrapper(bool isRegistered)
        {
            IsRegistered = isRegistered;
        }
    }
}
