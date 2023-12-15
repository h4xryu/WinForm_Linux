echo "Enter the project name : "
read project_name
mkdir ./${project_name}
cd ${project_name}
touch ${project_name}.cs




echo "using System;
using System.Drawing;
using System.Windows.Forms;

public class " > ${project_name}.cs 

echo ${project_name} >> ${project_name}.cs


echo ": Form

	public ()
	{
		
	}

{
    static public void Main()
    {
        Application.Run(new ());
    }
}    " >> ${project_name}.cs

	


exit
