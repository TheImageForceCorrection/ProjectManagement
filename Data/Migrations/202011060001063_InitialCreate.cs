namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Patronymic = c.String(),
                        Email = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CustomerCompanyName = c.String(),
                        ExecutorCompanyName = c.String(),
                        ProjectManagerId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.People", t => t.ProjectManagerId)
                .Index(t => t.ProjectManagerId);
            
            CreateTable(
                "dbo.ProjectDevelopers",
                c => new
                    {
                        Project_ID = c.Int(nullable: false),
                        Developer_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_ID, t.Developer_ID })
                .ForeignKey("dbo.Projects", t => t.Project_ID, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.Developer_ID, cascadeDelete: true)
                .Index(t => t.Project_ID)
                .Index(t => t.Developer_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ProjectManagerId", "dbo.People");
            DropForeignKey("dbo.ProjectDevelopers", "Developer_ID", "dbo.People");
            DropForeignKey("dbo.ProjectDevelopers", "Project_ID", "dbo.Projects");
            DropIndex("dbo.ProjectDevelopers", new[] { "Developer_ID" });
            DropIndex("dbo.ProjectDevelopers", new[] { "Project_ID" });
            DropIndex("dbo.Projects", new[] { "ProjectManagerId" });
            DropTable("dbo.ProjectDevelopers");
            DropTable("dbo.Projects");
            DropTable("dbo.People");
        }
    }
}
