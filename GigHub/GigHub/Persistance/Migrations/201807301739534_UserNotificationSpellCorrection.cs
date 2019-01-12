namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserNotificationSpellCorrection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserNotifications", "NotificaitonId", "dbo.Notifications");
            DropIndex("dbo.UserNotifications", new[] { "NotificaitonId" });
            AddColumn("dbo.UserNotifications", "Notification_Id", c => c.Int());
            CreateIndex("dbo.UserNotifications", "Notification_Id");
            AddForeignKey("dbo.UserNotifications", "Notification_Id", "dbo.Notifications", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "Notification_Id", "dbo.Notifications");
            DropIndex("dbo.UserNotifications", new[] { "Notification_Id" });
            DropColumn("dbo.UserNotifications", "Notification_Id");
            CreateIndex("dbo.UserNotifications", "NotificaitonId");
            AddForeignKey("dbo.UserNotifications", "NotificaitonId", "dbo.Notifications", "Id", cascadeDelete: true);
        }
    }
}
