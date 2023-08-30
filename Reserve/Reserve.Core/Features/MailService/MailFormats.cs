using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Reserve.Core.Features.MailService;

public static class MailFormats
{
    public static string EventCreationNotification(string eventId)
    {
        string response = "<h1 class=\"notification-title mb-lg-5\">Event Created Successfully!</h1>";
        response += "<div class=\"d-flex flex-column justify-content-center align-items-center\">";
        response += "<div class=\"notification-border mb-lg-5\">";
        response += "<div>";
        response += "<p class=\"notification-text d-flex justify-content-center mb-1\">You can now share your Event on the following link:</p>";
        response +=  $"<a class=\"d-flex justify-content-center mb-md-2\" href=\"https://localhost:7187/Event/EventDetails/{eventId}\">www.reserve.com/event/{eventId}</a>";
        response += "</div>";
        response += "<div>";
        response += "<p class=\"notification-text d-flex justify-content-center mb-1\">If you don’t have an account, you can still edit your event at";
        response += " the following link:";
        response += "</p>";
        response += $"<a class=\"d-flex justify-content-center mb-md-2\" href=\"https://localhost:7187/Event/ViewAttendees/{eventId}\">www.reserve.com/event/edit-event/{eventId}</a>";
        response += "</div>";
        response += "</div>";
        response += "</div>";
        return response;
    }
    public static string ReservationSuccessfulNotification(string ticketId)
    {
        string response = "<h1 class=\"notification-title mb-lg-5\">Reservation Successful!</h1>";
        response += "<div class=\"d-flex flex-column justify-content-center align-items-center\">";
        response += "<div class=\"notification-border mb-lg-5\">";
        response += "<div>";
        response +=  "<p class=\"notification-text d-flex justify-content-center mb-1\">You can view Event details or cancel reservation at the below link.</p>";
        response += $"<a class=\"d-flex justify-content-center mb-md-2\" href=\"https://localhost:7187/Event/ReserverDetails/{ticketId}\")>";
        response += $"www.reserve.com/Event/ReserverDetails/{ticketId}</a>";
        response += "</div>";
        response += "<div>";
        response += "<p class=\"notification-text d-flex justify-content-center align-content-center\">An email has been sent with the link.</p>";
        response += "</div>";
        response += "</div>";
        response += "</div>";
        return response;
    }
}
