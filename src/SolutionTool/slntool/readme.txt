F:\_E_\SolutionTool\output\Debug>slntool.exe
slntool 1.0.0.0
Copyright c Hewlett-Packard Company 2013

ERROR(S):
  -r/--repository required option is missing.


  -r, --repository     Required. The path of the repository.

  -t, --template       (Default: default.xml) The xml template file name for
                       checking solution structure.

  -b, --builds         (Default: ./output/$(Configuration)/) The BuildOutput
                       path.

  -i, --inspectcode    The InspectCode executable path.

  -l, --log            The log file path.

  --help               Display this help screen.



F:\_E_\SolutionTool\output\Debug>slntool.exe -r "F:\_E_\SolutionTool"
Begin checking F:\_E_\SolutionTool
***File Structure Rule***
--------------------------------------------------------------------------------
Directory Exists
.\src\SolutionTool\Modules\ManageRule\bin
.\src\SolutionTool\Modules\ManageTemplate\bin
--------------------------------------------------------------------------------
Directory Missing
.\deployment
--------------------------------------------------------------------------------
File Missing
.\output\debug\fileNotExists.txt
.\fileNotExists.txt



***Output Path Rule***
--------------------------------------------------------------------------------
Path OK
.\src\SolutionTool\slntool\slntool.csproj
..\..\..\output\$(Configuration)\ <--> ../../../output/$(Configuration)/
--------------------------------------------------------------------------------
Path NG
.\src\SolutionTool\Modules\ManageRule\ManageRule.csproj




***Code Analysis Rule***
--------------------------------------------------------------------------------
.\src\SolutionTool\SolutionTool.sln: .\Reports\InspectCode_Report_20140108222407508.xml



F:\_E_\SolutionTool\output\Debug>slntool.exe -r "F:\_E_\SolutionTool" –t custom1.xml



F:\_E_\SolutionTool\output\Debug>slntool.exe -r "F:\_E_\SolutionTool" –i "d:\jc-tools\InspectCode.exe"



