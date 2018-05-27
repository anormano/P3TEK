;#DropDB P3TEKEasyTest

#Application P3TEK.Win
#Application P3TEK.Web

*FillForm
 User Name = Admin
*Action Log On

#IfDef P3TEK.Win

*Action About...
 
*Action OK

#EndIf

#IfDef P3TEK.Web
*Action Navigation(My Details)
#EndIf
