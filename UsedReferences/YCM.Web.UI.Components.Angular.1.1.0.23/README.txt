This template requires you to edit your Global.asax.cs file.

In App_Start method add as the last line of the method the following code

Log4netConfig.ConfigureLog4Net(Server);
