using AutoMapper;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebGrease.Css.Extensions;

namespace GigHub.Controllers.Api
{
    public class NotificationsController : ApiController
    {

        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        } 

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _unitOfWork.Notification.GetNewNotificationsFor(userId);         

            return notifications.Select(Mapper.Map<Notification,NotificationDto>);
        } 

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userId = User.Identity.GetUserId();
            var userNotifications = _unitOfWork.UserNotification.GetNewUserNotificationsFor(userId);

            // My soultion
            //foreach (var notification in userNotifications)
            //{
            //    notification.IsRead = true;
            //}

            userNotifications.ForEach(n => n.Read());
            _unitOfWork.Complete();

            return Ok();
        }
    }    
    
}