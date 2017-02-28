Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class AddUsertable
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Users",
                Function(c) New With
                    {
                        .Id = c.Long(nullable := False, identity := True),
                        .UserName = c.String(nullable := False, maxLength := 50),
                        .Password = c.Binary(nullable := False, maxLength := 32, fixedLength := true),
                        .Salt = c.Binary(nullable := False, maxLength := 64, fixedLength := true),
                        .EmailAddress = c.String(nullable := False, maxLength := 200)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Users")
        End Sub
    End Class
End Namespace
