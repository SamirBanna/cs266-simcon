To work with the Vision/Khepera setup:

1. Choose the Debug configuration (to the right of the green arrow button, to the left of "Any CPU"
2. Under the SimCon project properties, choose
    Output type:    "Console Application"
    Startup object: "CS266.SimCon.Controller.Program"
    
To work with the Robotics Studio Simulator Environment:

1. Choose the Simulator configuration.
2. Under the SimCon project properties, choose
    Output type:    "Class library"
3. Make sure that Build > Output directory is
      C:\Microsoft Robotics Studio (1.5)\bin\

*** IMPORTANT ***
If references to Microsoft.Xna.Framework are missing:

   1. Right-click on the project in the Solution Explorer pane and click Add Reference
   2.
     A. See if Microsoft.Xna.Framework is in the list under the .NET tab,
        and if it is, select it.
     B. If the assembly is missing, download Microsoft.Xna.Framework.dll from
        http://code.google.com/p/cs266-simcon/downloads and copy it into
        C:\Microsoft Robotics Studio (1.5)\bin\
        Then add a reference directly to the DLL file.
   