Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class AddIsActivepropertytoallowuserlockout
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Users", "IsActive", Function(c) c.Boolean(nullable:=False, defaultValue:=True))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Users", "IsActive")
        End Sub
    End Class
End Namespace
