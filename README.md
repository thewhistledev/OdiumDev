![Title](https://i.imgur.com/qeqNrVm.png)


This is a simple logger library for C# applications. It provides a Logger class with two methods: `Log()` and `SaveLogs()`.

# `Log()` Function
The `Log()` method allows you to log messages with different levels of severity, along with the name of the module where the message originated. Here's an example of how to use it:

```
Logger.Log("error", "MyModule", "This is an error message");
Logger.Log("error", "MyModule", "This is an error message with an exception object", exception);
```
The first parameter is the severity level (`"error"`). The second parameter is the name of the module where the message originated. The third parameter is the message itself and the optional fourth parameter is the exception object.

The messages are logged to a file called log.txt in the current directory.

If the severity level is "error", the method also creates a crash dump file with the current date and time as the filename, and writes the stack trace and error information to it.

# **License**
This project is licensed under the MIT License - see the [LICENSE](https://github.com/thewhistledev/OdiumDev/blob/master/LICENSE.txt) file for details.
