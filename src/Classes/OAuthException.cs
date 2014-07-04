﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FlickrNet
{
    /// <summary>
    /// An OAuth error occurred when calling one of the OAuth authentication flow methods.
    /// </summary>
    public class OAuthException : Exception
    {
        private readonly string mess;


        /// <summary>
        /// Constructor for the OAuthException class.
        /// </summary>
        /// <param name="response"></param>
        public OAuthException(string response)
            : base("OAuth Exception")
        {
            FullResponse = response;

            OAuthErrorPameters = UtilityMethods.StringToDictionary(response, 1);

            mess = "OAuth Exception occurred: " + OAuthErrorPameters["oauth_problem"];
        }

        /// <summary>
        /// Constructor for the OAuthException class.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="innerException"></param>
        public OAuthException(string response, Exception innerException) : base("OAuth Exception", innerException)
        {
            FullResponse = response;

            OAuthErrorPameters = UtilityMethods.StringToDictionary(response);

            mess = "OAuth Exception occurred: " + OAuthErrorPameters["oauth_problem"];
        }

        /// <summary>
        /// Constructor for the OAuthException class.
        /// </summary>
        /// <param name="innerException"></param>
        public OAuthException(Exception innerException) : base("OAuth Exception", innerException)
        {
            var exception = innerException as WebException;
            if (exception == null) return;

            var res = exception.Response as HttpWebResponse;
            if (res == null) return;

            using (var sr = new StreamReader(res.GetResponseStream()))
            {
                string response = sr.ReadToEnd();

                FullResponse = response;

                OAuthErrorPameters = UtilityMethods.StringToDictionary(response);
                mess = "OAuth Exception occurred: " + OAuthErrorPameters["oauth_problem"];
            }
        }

        /// <summary>
        /// The full response of the exception.
        /// </summary>
        public string FullResponse { get; set; }

        /// <summary>
        /// The list of error parameters returned by the OAuth exception.
        /// </summary>
        public Dictionary<string, string> OAuthErrorPameters { get; set; }

        /// <summary>
        /// The message for the exception.
        /// </summary>
        public override string Message
        {
            get { return mess; }
        }
    }
}