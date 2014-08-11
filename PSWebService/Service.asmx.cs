using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.Services;

namespace PSWebService
{
    /// <summary>
    /// Summary description
    /// </summary>
    [WebService(Namespace = "http://webservice.atos.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PSWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string sayHello()
        {
            return "Hello World!";
        }

        [WebMethod]
        public string sayHelloToNames(List<string> names)
        {
		    return "Hello " + createNameListString(names);
        }

    	private String createNameListString(List<string> names) {
	    	if (names == null || names.Count == 0) {
		    	return "Anonymous!";
		    }

		    StringBuilder nameBuilder = new StringBuilder();
		    
            for (int i = 0; i < names.Count; i++) {
			    if (i != 0 && i != names.Count - 1)
			    	nameBuilder.Append(", ");
			    else if (i != 0 && i == names.Count - 1)
				    nameBuilder.Append(" & ");

			    nameBuilder.Append(names[i]);
		    }

		    nameBuilder.Append("!");

            return nameBuilder.ToString();
	    }

        [WebMethod]
        public String executeDOSCommand(string command)
        {
            return exec(command);
        }

        [WebMethod]
        public string executePSCommand(string command)
        {
		    string cmd = "powershell -ExecutionPolicy RemoteSigned -noprofile -noninteractive "	+ command;
		    return exec(cmd);
	    }

        private string exec(string command)
        {
            string output = string.Empty;
            string error = string.Empty;
 
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/c " + command);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            processStartInfo.UseShellExecute = false;
 
            Process process = Process.Start(processStartInfo);
            using (StreamReader streamReader = process.StandardOutput)
            {
                output = streamReader.ReadToEnd();
            }
 
            using (StreamReader streamReader = process.StandardError)
            {
                error = streamReader.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            else
            {
                return output;
            }
        }
    }
}