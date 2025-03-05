using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace GeoHub.Attributes;

public class BasicAuthenticationAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //context.Result = new ForbidResult();
        var request = context.HttpContext.Request;
        var authorization = request.Headers.Authorization;

        if (string.IsNullOrEmpty(authorization))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!authorization.ToString().Contains("Basic"))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (authorization.ToString().Split(" ").Length != 2)
        {
            context.Result = new ForbidResult();
            return;
        }

        if (authorization.ToString().Split(" ")[1] != "")
        {
            context.Result = new ForbidResult();
            return;
        }

        Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(authorization.ToString().Split(" ")[1]);

        if (userNameAndPasword == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        string userName = userNameAndPasword.Item1;
        string password = userNameAndPasword.Item2;

        if(userName != "aaa" || password != "bbb")
        {
            context.Result = new ForbidResult();
            return;
        }
    }


    private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
    {
        byte[] credentialBytes;

        try
        {
            credentialBytes = Convert.FromBase64String(authorizationParameter);
        }
        catch (FormatException)
        {
            return null;
        }
        
        Encoding encoding = Encoding.ASCII;
        // Make a writable copy of the encoding to enable setting a decoder fallback.
        encoding = (Encoding)encoding.Clone();
        // Fail on invalid bytes rather than silently replacing and continuing.
        encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
        string decodedCredentials;

        try
        {
            decodedCredentials = encoding.GetString(credentialBytes);
        }
        catch (DecoderFallbackException)
        {
            return null;
        }

        if (String.IsNullOrEmpty(decodedCredentials))
        {
            return null;
        }

        int colonIndex = decodedCredentials.IndexOf(':');

        if (colonIndex == -1)
        {
            return null;
        }

        string userName = decodedCredentials.Substring(0, colonIndex);
        string password = decodedCredentials.Substring(colonIndex + 1);
        return new Tuple<string, string>(userName, password);
    }
}