using System;
using System.Json;
using System.Net;

namespace BigML
{
    /// <summary>
    /// Base class for response from BigML
    /// </summary>
    public partial class Response : Client
    {
        public Response() : base("", "")
        {

        }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        public HttpStatusCode Code
        {
            get
            {
                var code = Object.code;
                return code is JsonPrimitive
                    ? (HttpStatusCode)(int)code
                    : HttpStatusCode.InternalServerError;
            }
        }

        /// <summary>
        /// Full address.
        /// </summary>
        public Uri Location
        {
            get; internal set;
        }

        /// <summary>
        /// Payload (strongly typed access in derived types), only exposed for debugging.
        /// </summary>
        public dynamic Object
        {
            get; internal set;
        }

        /// <summary>
        /// One of the categories in the table of <see cref="Category"/> categories that help classify this resource according to the domain of application.
        /// </summary>
        public Category Category
        {
            get { return (Category)(int)Object.category; }
        }

        /// <summary>
        /// This is the date and time in which the resource was created with microsecond precision.
        /// </summary>
        public DateTimeOffset Created
        {
            get { return DateTimeOffset.Parse(Object.created); }
        }

        /// <summary>
        /// A text describing the resource. It can contain restricted markdown to decorate the text.
        /// </summary>
        public string Description
        {
            get { return Object.decription; }
        }

        /// <summary>
        /// The resource id
        /// </summary>
        public string Resource
        {
            get { return Object.resource; }
        }

        /// <summary>
        /// The id of this resource without the resource type
        /// </summary>
        public string Id
        {
            get {
                string[] splits = Resource.Split('/');
                if (splits.Length > 1)
                {
                    return splits[1];
                }
                return Resource;
            }
        }

        /// <summary>
        /// This is the date and time in which the resource was last updated with microsecond precision.
        /// </summary>
        public DateTimeOffset Updated
        {
            get { return DateTimeOffset.Parse((string)Object.updated); }
        }

        /// <summary>
        /// Whether the resource is public or not.
        /// </summary>
        public bool IsPrivate
        {
            get { return Object.@private; }
        }

        /// <summary>
        /// The numbers of credits that costed you to create this resource.
        /// </summary>
        public double Credits
        {
            get { return Object.credits; }
        }


        internal String ApiKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        internal String UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        internal String Dev
        {
            get { return _dev; }
            set { _dev = value; }
        }

        internal String VpcDomain
        {
            get { return _VpcDomain; }
            set { _VpcDomain = value; }
        }

        internal bool UseContextInAwaits
        {
            get { return _useContextInAwaits; }
            set { _useContextInAwaits = value; }
        }


    }
}