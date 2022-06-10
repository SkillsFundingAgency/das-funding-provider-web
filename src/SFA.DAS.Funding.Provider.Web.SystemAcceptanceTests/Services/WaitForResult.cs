﻿namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services
{
    public class WaitForResult
    {
        public WaitForResult()
        {
            HasTimedOut = false;
            HasCompleted = false;
            HasStarted = false;
            HasErrored = false;
            LastException = null!;
        }

        public bool HasTimedOut { get; private set; }

        public bool HasStarted { get; private set; }

        public bool HasErrored { get; private set; }

        public Exception LastException { get; private set; }

        public bool HasCompleted { get; private set; }

        public void SetHasTimedOut()
        {
            HasTimedOut = true;
        }

        public void SetHasCompleted()
        {
            HasCompleted = true;
        }

        public void SetHasStarted()
        {
            HasStarted = true;
        }

        public void SetHasErrored(Exception ex)
        {
            HasErrored = true;
            LastException = ex;
        }
    }
}
