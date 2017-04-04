using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace RESTfulBAL.Utilities
{
    [Serializable()]
    public class UserInvalidLoginException : Exception
    {
        private readonly int iUserID;
        public int UserID
        {
            get { return this.iUserID; }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        // Constructor should be protected for unsealed classes, private for sealed classes.
        // (The Serializer invokes this constructor through reflection, so it can be private)
        protected UserInvalidLoginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.iUserID = int.Parse(info.GetString("UserID"));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("UserID", this.iUserID);

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }

        public UserInvalidLoginException()
        {
        }

        public UserInvalidLoginException(string message)
            : base(message)
        {
        }

        public UserInvalidLoginException(string message, int UserID)
             : base(message)
        {
            this.iUserID = UserID;
        }

        public UserInvalidLoginException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public UserInvalidLoginException(string message, Exception inner, int UserID)
            : base(message, inner)
        {
            this.iUserID = UserID;
        }
    }

    [Serializable()]
    public class NoUserChangesException : Exception
    {
        private readonly int iUserID;
        public int UserID
        {
            get { return this.iUserID; }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        // Constructor should be protected for unsealed classes, private for sealed classes.
        // (The Serializer invokes this constructor through reflection, so it can be private)
        protected NoUserChangesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.iUserID = int.Parse(info.GetString("UserID"));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("UserID", this.iUserID);

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }

        public NoUserChangesException()
        {
        }

        public NoUserChangesException(string message)
            : base(message)
        {
        }

        public NoUserChangesException(string message, int UserID)
             : base(message)
        {
            this.iUserID = UserID;
        }

        public NoUserChangesException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NoUserChangesException(string message, Exception inner, int UserID)
            : base(message, inner)
        {
            this.iUserID = UserID;
        }
    }

    [Serializable()]
    public class NoUserCredentialsException : Exception
    {
        private readonly int iUserID;
        public int UserID
        {
            get { return this.iUserID; }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        // Constructor should be protected for unsealed classes, private for sealed classes.
        // (The Serializer invokes this constructor through reflection, so it can be private)
        protected NoUserCredentialsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.iUserID = int.Parse(info.GetString("UserID"));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("UserID", this.iUserID);

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }

        public NoUserCredentialsException()
        {
        }

        public NoUserCredentialsException(string message)
            : base(message)
        {
        }

        public NoUserCredentialsException(string message, int UserID)
             : base(message)
        {
            this.iUserID = UserID;
        }

        public NoUserCredentialsException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NoUserCredentialsException(string message, Exception inner, int UserID)
            : base(message, inner)
        {
            this.iUserID = UserID;
        }
    }
}
