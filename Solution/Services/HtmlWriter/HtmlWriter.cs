using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Services.HtmlWriter
{
    public static class HtmlWriter
    {
        public static string A(string link, string content)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<a href='").Append(link).Append("'>");
            stringBuilder.Append(content);
            stringBuilder.Append("</a>");

            return stringBuilder.ToString();
        }

        public static string Br()
        {
            return "<br/>";
        }

        public static string H(string text, int headerSize = 1)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<h").Append(headerSize).Append(">")
                .Append(text)
                .Append("</h").Append(headerSize).Append(">");

            return stringBuilder.ToString();
        }

        public static string PromoActionNotificationTemplate(string name, PromoAction promoAction)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<h1>New promo action!</h1>");
            stringBuilder.Append("<blockquote>");

            stringBuilder.Append("A new promo action has been added for the service you are subscribed to!")
                .Append("<br/>");
            stringBuilder.Append("Service name - ").Append(name).Append("<br/>");
            stringBuilder.Append("Some of the details -<br/>");
            stringBuilder.Append("<ul>");

            stringBuilder.Append("<li> Price per day : ").Append(promoAction.PricePerDay).Append("</li>");
            stringBuilder.Append("<li> Capacity : ").Append(promoAction.Capacity).Append("</li>");
            stringBuilder.Append("<li> Start date: ").Append(promoAction.StartDateTime.ToString("D")).Append("</li>");
            stringBuilder.Append("<li> End date : ").Append(promoAction.EndDateTime.ToString("D")).Append("</li>");

            stringBuilder.Append("</ul>");
            stringBuilder.Append("</blockquote>");
            stringBuilder.Append("<h3>For further details go and check it out on the site!</h3>");

            return stringBuilder.ToString();
        }

        public static string ReservationNotificationTemplate(Reservation reservation, Service service)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder
                .Append("<h3>New reservation!</h3>")
                .Append("<p> Dear user,</p>")
                .Append(
                    "<p> We are writing to inform you that a new reservation has been added to your calendar!The reservation specification is state bellow. If you are not expecting to have that reservation, contact the admins.</p>")
                .Append("<p> Reservation details:</p>")
                .Append("<ul>")
                .Append("<li><strong> Service name </strong>:").Append(service.Name).Append("</li>")
                .Append("<li><strong> Creation date </strong>:").Append(reservation.ReservedDateTime).Append("</li>")
                .Append("<li><strong> Price per day</strong>:").Append(reservation.Price).Append("</li>")
                .Append("<li><strong> Start date</strong>:").Append(reservation.StartDateTime).Append("</li>")
                .Append("<li><strong> End date </strong>:").Append(reservation.EndDateTime).Append("</li>")
                .Append("</ul>")
                .Append("<p> Some additional equipment:&nbsp;").Append(reservation.AdditionalEquipment).Append("</p>")
                .Append("<p> &nbsp;</p>")
                .Append("<p><em> Yours faithfully, ZAMISR </em></p>");
            
            return stringBuilder.ToString();
        }
    }
}
