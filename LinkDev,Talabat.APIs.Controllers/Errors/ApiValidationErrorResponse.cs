﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public required IEnumerable<ValidationError> Errors { get; set; }

        public ApiValidationErrorResponse(string? message = null) : base(400, message)
        {

        }


        // nested class , if we will use it inside ApiValidationErrorResponse only
        public class ValidationError
        {
            public required string Filed { get; set; }

            public required  IEnumerable<string> Errors { get; set; }

        }
    }
}
