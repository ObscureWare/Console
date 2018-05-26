namespace OsInfo
{
    using Microsoft.Win32;

    public static class RegistryHelpers
    {
        public static string RegistryRead(string registryPath, string field, string defaultValue)
        {
            string rtn = "";
            string backSlash = "";
            string newRegistryPath = "";

            try
            {
                RegistryKey ourKey = null;
                string[] splitResult = registryPath.Split('\\');

                if (splitResult.Length > 0)
                {
                    splitResult[0] = splitResult[0].ToUpper(); // Make the first entry uppercase...

                    if (splitResult[0] == "HKEY_CLASSES_ROOT") ourKey = Registry.ClassesRoot;
                    else if (splitResult[0] == "HKEY_CURRENT_USER") ourKey = Registry.CurrentUser;
                    else if (splitResult[0] == "HKEY_LOCAL_MACHINE") ourKey = Registry.LocalMachine;
                    else if (splitResult[0] == "HKEY_USERS") ourKey = Registry.Users;
                    else if (splitResult[0] == "HKEY_CURRENT_CONFIG") ourKey = Registry.CurrentConfig;

                    if (ourKey != null)
                    {
                        for (int i = 1; i < splitResult.Length; i++)
                        {
                            newRegistryPath += backSlash + splitResult[i];
                            backSlash = "\\";
                        }

                        if (newRegistryPath != "")
                        {
                            //rtn = (string)Registry.GetValue(RegistryPath, "CurrentVersion", DefaultValue);

                            ourKey = ourKey.OpenSubKey(newRegistryPath);
                            if (ourKey != null)
                            {
                                rtn = (string)ourKey.GetValue(field, defaultValue);
                                ourKey.Close();
                            }
                        }
                    }
                }
            }
            catch
            {
                throw; // test it and use properly
            }

            return rtn;
        }
    }
}