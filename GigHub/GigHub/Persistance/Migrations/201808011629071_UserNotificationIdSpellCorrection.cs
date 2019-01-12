namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserNotificationIdSpellCorrection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserNotifications", "Notification_Id", "dbo.Notifications");
            DropIndex("dbo.UserNotifications", new[] { "Notification_Id" });
            RenameColumn(table: "dbo.UserNotifications", name: "Notification_Id", newName: "NotificationId");
            DropPrimaryKey("dbo.UserNotifications");
            AlterColumn("dbo.UserNotifications", "NotificationId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.UserNotifications", new[] { "UserId", "NotificationId" });
            CreateIndex("dbo.UserNotifications", "NotificationId");
            AddForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications", "Id", cascadeDelete: true);
            DropColumn("dbo.UserNotifications", "NotificaitonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserNotifications", "NotificaitonId", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropPrimaryKey("dbo.UserNotifications");
            AlterColumn("dbo.UserNotifications", "NotificationId", c => c.Int());
            AddPrimaryKey("dbo.UserNotifications", new[] { "UserId", "NotificaitonId" });
            RenameColumn(table: "dbo.UserNotifications", name: "NotificationId", newName: "Notification_Id");
            CreateIndex("dbo.UserNotifications", "Notification_Id");
            AddForeignKey("dbo.UserNotifications", "Notification_Id", "dbo.Notifications", "Id");
        }
    }
}
