using System.Diagnostics;
using Newtonsoft.Json;
using Retrowarden.Models;

namespace Retrowarden.Utils
{
    public sealed class VaultProxy
    {
        private readonly Process _bwcli;
        private String _response;
        private String _error;
        private String _sessionKey;
        private String _JSONstring;
        private bool _cmdExit;
        private bool _isLoggedIn;
        private String _cmdExitCode;

        public VaultProxy(string bwExeLocation)
        {
            // Set defaults.
            _response = "";
            _cmdExit = false;
            _cmdExitCode = "";
            _isLoggedIn = false;
            
            // Create exe caller.
            _bwcli = new Process();
            _bwcli.StartInfo.FileName = bwExeLocation;

            // Set call options
            _bwcli.EnableRaisingEvents = true;
            _bwcli.StartInfo.UseShellExecute = false;
            _bwcli.StartInfo.RedirectStandardError = true;
            _bwcli.StartInfo.RedirectStandardOutput = true;

            // Attach events.
            _bwcli.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(proxyCmd_DidReceiveData);
            _bwcli.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(proxyCmd_DidReceiveError);
            _bwcli.Exited += new System.EventHandler(proxyCmd_DidExit);
        }

        public void Login(string email, string password)
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("login");
            _bwcli.StartInfo.ArgumentList.Add(email);
            _bwcli.StartInfo.ArgumentList.Add(password);

            // Execute.
            ExecuteCommand();

            // Get session key from response.
            string marker = "--session ";

            // Make sure the marker is in the response.
            if (_response.Contains(marker))
            {
                // Find the start of the key
                int pos = _response.IndexOf(marker, 0) + marker.Length;

                // Store the session key for future calls.
                _sessionKey = _response.Substring(pos);
                
                // Set env variable.
                _bwcli.StartInfo.Environment.Add("BW_SESSION", _sessionKey);
                
                // At this point we are logged in.
                _isLoggedIn = true;
            }
        }
        
        public void Lock()
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("lock");

            // Execute.
            ExecuteCommand();
        }
        
        public void Logout()
        {
            // Check to make sure we are logged in.
            if (_isLoggedIn)
            {
                // Add parameters for call.
                _bwcli.StartInfo.ArgumentList.Add("logout");
                
                // Execute.
                ExecuteCommand();
                
                // Flip login bit.
                _isLoggedIn = false;
            }

        }
        
        public List<VaultFolder> ListFolders()
        {
            // Return object.
            List<VaultFolder> retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("list");
            _bwcli.StartInfo.ArgumentList.Add("folders");

            // Execute.
            ExecuteCommand();
          
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
            
                // Get item list.
                retVal = JsonConvert.DeserializeObject<List<VaultFolder>>(_response,settings);
            }
            
            // Return list.
            return retVal;
        }

        public List<VaultCollection> ListCollections()
        {
            // Return value.
            List<VaultCollection> retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("list");
            _bwcli.StartInfo.ArgumentList.Add("collections");

            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Set serialization rules.
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
            
                // Get item list.
                retVal = JsonConvert.DeserializeObject<List<VaultCollection>>(_response,settings);
            }
            
            // Return list.
            return retVal;
        }
        
        public List<Organization> ListOrganizations()
        {
            // Return value.
            List<Organization> retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("list");
            _bwcli.StartInfo.ArgumentList.Add("organizations");

            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Set serialization rules.
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
            
                // Get item list.
                retVal = JsonConvert.DeserializeObject<List<Organization>>(_response,settings);
            }
            
            // Return list.
            return retVal;
        }
        
        public SortedDictionary<string, VaultItem> ListVaultItems()
        {
            // Return object.
            SortedDictionary<string, VaultItem> retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("list");
            _bwcli.StartInfo.ArgumentList.Add("items");

            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Set serialization rules.
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
            
                // Get item list.
                List<VaultItem> items  = JsonConvert.DeserializeObject<List<VaultItem>>(_response,settings);
                
                // Migrate to a dictionary.
                retVal = new SortedDictionary<string, VaultItem>(items.ToDictionary(keySelector: m => m.Id, 
                    elementSelector: m => m));
            }
            
            // Return it.
            return retVal;
        }

        public VaultItem CreateVaultItem(string encodedItem)
        {
            // The return value.
            VaultItem retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("create");
            _bwcli.StartInfo.ArgumentList.Add("item");
            _bwcli.StartInfo.ArgumentList.Add(encodedItem);

            // Execute.
            ExecuteCommand();
    
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Set serialization rules.
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;

                // Get item list.
                retVal = JsonConvert.DeserializeObject<VaultItem>(_response, settings);
            }
            
            // Return saved object.
            return retVal;
        }

        public VaultItem UpdateVaultItem(string id, string encodedItem)
        {
            // The return value.
            VaultItem retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("edit");
            _bwcli.StartInfo.ArgumentList.Add("item");
            _bwcli.StartInfo.ArgumentList.Add(id);
            _bwcli.StartInfo.ArgumentList.Add(encodedItem);

            // Execute.
            ExecuteCommand();
    
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Set serialization rules.
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;

                // Get item list.
                retVal = JsonConvert.DeserializeObject<VaultItem>(_response, settings);
            }
            
            // Return saved object.
            return retVal;
        }

        public void DeleteVaultItem(string itemId)
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("delete");
            _bwcli.StartInfo.ArgumentList.Add("item");
            _bwcli.StartInfo.ArgumentList.Add(itemId);

            // Execute.
            ExecuteCommand();
    
        }
        
        private void ExecuteCommand(bool stdinUsed=false)
        {
            // Reset command flags & response.
            _cmdExit = false;
            _cmdExitCode = "";
            _response = "";
            _error = "";

            // Execute.
            _bwcli.Start();
            
            // Check to see if we should be sending data in.
            if (stdinUsed)
            {
                // Write JSON to stdin.
                StreamWriter itemWriter = _bwcli.StandardInput;
                itemWriter.Write(_JSONstring);
            }
            _bwcli.BeginErrorReadLine();
            _bwcli.BeginOutputReadLine();

            // Block until finished.
            _bwcli.WaitForExit();
            
            // Close streams.
            _bwcli.CancelErrorRead();
            _bwcli.CancelOutputRead();
            
            // Reset arguments.
            _bwcli.StartInfo.ArgumentList.Clear();
        }

        #region Event Handlers
        private void proxyCmd_DidExit(object sender, EventArgs e)
        {
            // Flag that data has all been received.
            _cmdExit = true;
            _cmdExitCode = _bwcli.ExitCode.ToString();
        }

        private void proxyCmd_DidReceiveError(object sender, DataReceivedEventArgs e)
        {
            _error += e.Data;
        }

        private void proxyCmd_DidReceiveData(object sender, DataReceivedEventArgs e)
        {
            // Append to response.
            _response += e.Data;
        }
        #endregion
        
        #region Properties
        public string ExitCode
        {
            get
            {
                return _cmdExitCode;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _error;
            }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
        }

        #endregion
    }
}