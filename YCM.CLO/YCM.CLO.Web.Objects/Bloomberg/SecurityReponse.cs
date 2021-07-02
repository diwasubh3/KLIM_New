using System.Collections.Generic;

namespace YCM.CLO.Web.Objects
{
	public class SecurityReponse
    {
        public string Security { get; set; }

        public IList<FieldResponse> FieldResponses { get; set; }
        public IList<FieldExceptionResponse> FieldExceptions { get; set; }

        public SecurityErrorResponse ErrorResponse { get; set; }

        public SecurityReponse()
        {
            FieldResponses = new List<FieldResponse>();
            FieldExceptions = new List<FieldExceptionResponse>();
        }
    }
}
